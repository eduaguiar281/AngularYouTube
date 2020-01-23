import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
    selector: 'app-video',
    templateUrl: './video.component.html',
    styleUrls: ['./video.component.css']
})
export class VideoComponent {
    constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string, private route: ActivatedRoute,
        private router: Router) {
        this._baseUrl = baseUrl;
        this._http = http;
        this.loading = false;
        this.pageSize = 5;
        this.pageIndex = 0;


        if (this.route.snapshot.paramMap.get('pageSize') != null ||
            this.route.snapshot.paramMap.get('pageIndex') != null ||
            this.route.snapshot.paramMap.get('searchQuery') != null) {
            this.pageSize = this.route.snapshot.paramMap.get('pageSize') ? this.route.snapshot.paramMap.get('pageSize') : 5;
            this.pageIndex = this.route.snapshot.paramMap.get('pageIndex') ? this.route.snapshot.paramMap.get('pageIndex') : 0;
            this.searchQuery = this.route.snapshot.paramMap.get('searchQuery');
            this.startSearch();
        }


    }

    _http: HttpClient;
    _baseUrl: string;
    public resposta: ResponseVideo;
    public searchQuery: string;
    public pageSize: number;
    public pageIndex: number;
    public displayPageIndex: number;
    public loading: boolean;
   
    public onPageSizeChange(newValue) {
        this.pageSize = newValue;
        this.pageIndex = 0;
        this.startSearch();
    }

    public nextPage() {
        if (!this.resposta || !this.resposta.hasNextPage)
            return;
        this.pageIndex++;
        this.startSearch();
    }

    public priorPage() {
        if (!this.resposta || !this.resposta.hasPreviousPage)
            return;
        this.pageIndex--;
        this.startSearch();
    }

    public startSearch() {
        this.loading = true;
        var service = 'api/v1/Video/?pageSize=' + this.pageSize;// + this.searchQuery;
        if (this.pageIndex > 0) {
            service = service + '&pageIndex=' + this.pageIndex;
        }
        if (this.searchQuery != null) {
            service = service + '&queryString=' + this.searchQuery;
        }

        this._http.get<ResponseVideo>(this._baseUrl + service).subscribe(result => {
            this.resposta = result;
            this.displayPageIndex = result.pageIndex + 1;
            this.pageIndex = result.pageIndex;
            this.loading = false;
        }, error => {
            console.error(error);
            alert('Ocorreu um erro inesperado ao pesquisar videos!');
            this.loading = false;
        });

    }

    gotoDetails(video: Video) {
        let videoId = video ? video.id : null;

        if (this.searchQuery != null) {
            this.router.navigate(['/video-details', { id: videoId, pageSize: this.pageSize, pageIndex: this.pageIndex, searchQuery: this.searchQuery }]);
        }
        else {
            this.router.navigate(['/video-details', { id: videoId, pageSize: this.pageSize, pageIndex: this.pageIndex }]);
        }
    }

    truncateText(value, maxLength) {
        let truncated = value;

        if (truncated.length > maxLength) {
           truncated = truncated.substr(0, maxLength) + '...';
        } 
        return truncated;
    }

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

export interface Video {
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
    idioma: string;
    definicao: string;
}
