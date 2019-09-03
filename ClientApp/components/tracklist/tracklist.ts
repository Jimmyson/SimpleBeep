import Vue from 'vue';
import Axios from 'axios';
import { Component } from 'vue-property-decorator';

class Playlist 
{
    name: string;
    composer: string;
    id: string; // @todo: FIND GUID SUPPLIMENT

    constructor() {
        this.name = "";
        this.composer = "";
        this.id = "";
    }
}

@Component
export default class TracklistComponent extends Vue {
    Tracklists: Playlist[] = [];
    createItem: Playlist = new Playlist();
    errors: any[] = [];

    mounted() {
        Axios.get('api/Playlist')
            .then(response => {
                this.Tracklists = response.data
            })
            .catch(e => {
                this.errors.push(e);
            });
    }

    createPlaylist() {
        Axios.post('api/Playlist', this.createItem)
            .then(respone => {})
            .then(action => {
                this.Tracklists.push(this.createItem)
            })
            .then(action => this.createItem = new Playlist)
            .catch(e => {
                this.errors.push(e);
            })
    }

    updatePlaylist() {
        Axios.put('api/Track' + '<id>', {
            body: undefined //this.Tracklists[x]
        })
            .then(response => {})
            .catch(e => {
                this.errors.push(e)
            })
    }

    deletePlayliist() {
        Axios.delete('api/Playlist' + '<id>')
            .then(response => {})
            .catch(e => {
                this.errors.push(e);
            })
    }
}