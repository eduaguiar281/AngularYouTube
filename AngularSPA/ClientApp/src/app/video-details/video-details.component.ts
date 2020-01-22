import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';
import { Video } from '../video/video.component';

@Component({
    selector: 'app-video-details',
    templateUrl: './video-details.component.html',
    styleUrls: ['../video/video.component.css']
})
export class VideoDetailsComponent {
    constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string, private route: ActivatedRoute,
        private router: Router) {
        this._baseUrl = baseUrl;
        this._http = http;
        this.loading = false;
        this.hasVideo = false;

        this.id = this.route.snapshot.paramMap.get('id');
        this.pageSize = this.route.snapshot.paramMap.get('pageSize');
        this.pageIndex = this.route.snapshot.paramMap.get('pageIndex');
        this.searchQuery = this.route.snapshot.paramMap.get('searchQuery');

        this.service = 'api/v1/Video/' + this.id;// + this.searchQuery;
        this._http.get<ResponseVideo>(this._baseUrl + this.service).subscribe(result => {
            this.respostaVideo = result;
            this.loading = false;
        }, error => {
            console.error(error);
            alert('Ocorreu um erro inesperado ao pesquisar canal!');
            this.loading = false;
        });
    }

    backClicked() {
        if (this.searchQuery == null) {
            this.router.navigate(['/video', { pageSize: this.pageSize, pageIndex: this.pageIndex }]);
        }
        else {
            this.router.navigate(['/video', { pageSize: this.pageSize, pageIndex: this.pageIndex, searchQuery: this.searchQuery }]);
        }
    }

    _http: HttpClient;
    _baseUrl: string;
    public respostaVideo: ResponseVideo;
    public loading: boolean;
    public id: string;

    public searchQuery: string;
    public pageSize: number;
    public pageIndex: number;
    public service: string;
    public hasVideo: boolean;

}

interface ResponseVideo {
    data: Video;
    success: boolean;
    message: string;
    pageIndex: number;
    pageSize: string;
    totalCount: number;
    totalPages: number;
    hasPreviousPage: number;
    hasNextPage: number;
}

