import Vue from 'vue';
import Axios from 'axios';
import { Component } from 'vue-property-decorator';

class Playlist 
{
    name: string;
    composer: string;
    id: string;
    trackCount: number;
    edit: boolean;

    constructor() {
        this.name = "";
        this.composer = "";
        this.id = "";
        this.trackCount = 0;
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

    sortList(propName: keyof Playlist, order: string) {
        var p = new Playlist();
        switch (typeof p[propName]) {
            case "string":
                this.Tracklists.sort((a,b) => {
                    var x = a[propName].toString().toLowerCase();
                    var y = b[propName].toString().toLowerCase();
                    if (x < y) {return -1;}
                    if (x > y) {return 1;}
                    return 0;
                })
                break;
            default:
                this.Tracklists.sort((a,b) => {
                    if (a[propName] < b[propName]) {return -1;}
                    if (a[propName] > b[propName]) {return 1;}
                    return 0;
                })
                break;
        };

        if (order === 'DESC') this.Tracklists.reverse();
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
        if (this.createItem.name === "") {
            this.errors.push("Please enter in a Tracklist name");
            return;
        }

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

    dismissNotice(loc: number) {
        this.errors.splice(loc, 1);
    }
}