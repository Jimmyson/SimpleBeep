import Vue from 'vue';
import Axios from 'axios';
import { Component } from 'vue-property-decorator';

class Playlist 
{
    name: string;
    composer: string;
    id: string; // @todo: FIND GUID SUPPLIMENT
    edit: boolean;

    constructor() {
        this.name = "";
        this.composer = "";
        this.id = "";
        this.edit = false;
    }
}

@Component
export default class TracklistComponent extends Vue {
    Tracklists: Playlist[] = [];
    createItem: Playlist = new Playlist();
    errors: any[] = [];

    queryList() {
        Axios.get('api/Playlist')
            .then(response => {
                this.Tracklists = response.data;
                this.Tracklists.forEach((item) => {
                    item.edit = false
                })
            })
            .catch(e => {
                this.errors.push(e);
            });
    }

    editPlaylist(loc: number)
    {
        this.Tracklists[loc].edit = !this.Tracklists[loc].edit;
        this.$forceUpdate(); // Used to re-render Vue UI
    }

    mounted() {
        this.queryList();
    }

    createPlaylist() {
        Axios.post('api/Playlist', this.createItem)
            .then(response => {
                this.queryList();
            })
            .then(action => this.createItem = new Playlist)
            .catch(e => {
                this.errors.push(e);
            })
    }

    updatePlaylist(loc: number, id: string) {
        var item = this.Tracklists[loc];
        delete item.edit;
        Axios.put('api/Playlist/' + id, item)
            .then(response => {
                this.Tracklists[loc].edit = false;
                this.queryList();
            })
            .catch(e => {
                this.errors.push(e)
            })
    }

    deletePlaylist(loc: number, id: string) {
        Axios.delete('api/Playlist/' + id)
            .then(response => {
                this.Tracklists[loc].edit = false;
                this.queryList();
            })
            .catch(e => {
                this.errors.push(e);
            })
    }
}