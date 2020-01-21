import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
    selector: 'app-canal-details',
    templateUrl: './canal-details.component.html'
})
export class CanalDetailsComponent {
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

        this.service = 'api/v1/Canal/' + this.id;// + this.searchQuery;
        this._http.get<ResponseCanal>(this._baseUrl + this.service).subscribe(result => {
            this.respostaCanal = result;
            this.loading = false;
        }, error => {
            console.error(error);
            alert('Ocorreu um erro inesperado ao pesquisar canal!');
            this.loading = false;
        });

        this._http.get<ResponseVideo>(this._baseUrl + this.service + '/videos').subscribe(result => {
            this.respostaVideo = result;
            this.loading = false;
            this.hasVideo = result.data.length > 0;
        }, error => {
            console.error(error);
            alert('Ocorreu um erro inesperado ao pesquisar videos!');
                this.loading = false;
                this.hasVideo = false;
        });
    }

    backClicked() {
        if (this.searchQuery == null) {
            this.router.navigate(['/canal', { pageSize: this.pageSize, pageIndex: this.pageIndex }]);
        }
        else {
            this.router.navigate(['/canal', { pageSize: this.pageSize, pageIndex: this.pageIndex, searchQuery: this.searchQuery }]);
        }
    }

    _http: HttpClient;
    _baseUrl: string;
    public respostaCanal: ResponseCanal;
    public respostaVideo: ResponseVideo;
    public loading: boolean;
    public id: string;

    public searchQuery: string;
    public pageSize: number;
    public pageIndex: number;
    public service: string;
    public hasVideo: boolean;

}

interface ResponseCanal {
    data: Canal;
    success: boolean;
    message: string;
    pageIndex: number;
    pageSize: string;
    totalCount: number;
    totalPages: number;
    hasPreviousPage: number;
    hasNextPage: number;
}

interface Canal {
    id: string;
    channelId: string;
    title: string;
    descricao: string;
    imagem: string;
    publicadoEm: Date;
}

interface ResponseVideo {
    data: Video[];
    success: boolean;
    message: string;
    pageIndex: number;
    pageSize: string;
    totalCount: number;
    totalPages: number;
    hasPreviousPage: number;
    hasNextPage: number;
}

interface Video {
    id: string;
    videoId: string;
    canalId: string;
    titulo: string;
    descricao: string;
    publicadoEm: string;
    imagemUrl: string;
    quantidadeLike: number;
    quantidadeDeslike: number;
    quantidadeComentario: number;
    quantidadeVisualizacao: number;
}
