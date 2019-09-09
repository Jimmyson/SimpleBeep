import Vue from 'vue';
import Axios from 'axios';
import { Component } from 'vue-property-decorator';

class Track {
    name: string;
    description: string;
    id: string;
    playlist: string;
    edit: boolean;

    constructor() {
        this.name = "";
        this.description = "";
        this.id = "";
        this.playlist = "";
        this.edit = false;
    }
}

class PlaylistTracks 
{
    name: string;
    composer: string;
    id: string;
    tracks: Track[];

    constructor() {
        this.name = "";
        this.composer = "";
        this.id = "";
        this.tracks = [];
    }
}

@Component
export default class TracklistEditorComponent extends Vue {
    Tracklist: PlaylistTracks = new PlaylistTracks();
    createItem: Track = new Track();
    errors: any[] = [];

    queryTracklist() {
        Axios.get('api/Playlist/Tracks/' + this.$route.params.id)
            .then(response => {
                this.Tracklist = response.data;
            })
            .catch(e => {
                this.errors.push(e);
            });
    }

    sortList(propName: keyof Track, order: string) {
        var p = new Track();
        switch (typeof p[propName]) {
            case "string":
                this.Tracklist.tracks.sort((a,b) => {
                    var x = a[propName].toString().toLowerCase();
                    var y = b[propName].toString().toLowerCase();
                    if (x < y) {return -1;}
                    if (x > y) {return 1;}
                    return 0;
                })
                break;
            default:
                this.Tracklist.tracks.sort((a,b) => {
                    if (a[propName] < b[propName]) {return -1;}
                    if (a[propName] > b[propName]) {return 1;}
                    return 0;
                })
                break;
        };

        if (order === 'DESC') this.Tracklist.tracks.reverse();
    }

    editTrack(loc: number)
    {
        this.Tracklist.tracks[loc].edit = !this.Tracklist.tracks[loc].edit;
        this.$forceUpdate(); // Used to re-render Vue UI
    }

    mounted() {
        this.createItem.playlist = this.$route.params.id;
        this.queryTracklist();
    }

    createTrack() {
        if (this.createItem.name === "") {
            this.errors.push("Please enter in a Tracklist name");
            return;
        }

        Axios.post('api/Track', this.createItem)
            .then(response => {
                response.data.edit = false;
                response.data.playlist = response.data.playlistId;
                delete response.data.playlistId;
                this.Tracklist.tracks.push(response.data as Track)
            })
            .then(action => {
                this.createItem = new Track();
                this.createItem.playlist = this.$route.params.id;
            })
            .catch(e => {
                this.errors.push(e);
            })
    }

    updateTrack(loc: number, id: string) {
        var item = this.Tracklist.tracks[loc];
        delete item.edit;
        Axios.put('api/Track/' + id, item)
            .then(response => {
                this.Tracklist.tracks[loc].edit = false;
            })
            .catch(e => {
                this.errors.push(e)
            })
        this.$forceUpdate();
    }

    deleteTrack(loc: number, id: string) {
        Axios.delete('api/Track/' + id)
            .then(response => {
                this.Tracklist.tracks[loc].edit = false;
                this.Tracklist.tracks.splice(loc, 1);
            })
            .catch(e => {
                this.errors.push(e);
            })
    }

    dismissNotice(loc: number) {
        this.errors.splice(loc, 1);
    }
}