import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
    selector: 'app-youtube-search',
    templateUrl: './youtube-search.component.html'
})
export class YoutubeSearchComponent {
    constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        this._baseUrl = baseUrl;
        this._http = http;
        this.pageToken = null;
        this.loading = false;
    }
    _http: HttpClient;
    _baseUrl: string;
    public resposta: ResultSet;
    public searchQuery: string;
    public pageToken: string;
    public hasNext: boolean;
    public hasPrior: boolean;
    public loading: boolean;

    public nextPage() {
        if (!this.resposta || !this.resposta.data || !this.resposta.data.nextPageToken)
            return;
        this.pageToken = this.resposta.data.nextPageToken;
        this.startSearch();
    }

    public priorPage() {
        if (!this.resposta || !this.resposta.data || !this.resposta.data.priorPageToken)
            return;
        this.pageToken = this.resposta.data.priorPageToken;
        this.startSearch();
    }

    public startSearch() {
        if (!this.searchQuery || this.searchQuery === '') {
            alert('Informe os dados da pesquisa!');
            return;
        }
        this.loading = true;
        var service = 'api/v1/YoutubeSearch/' + this.searchQuery;
        if (this.pageToken)
            service = service + '/' + this.pageToken;
        this._http.get<ResultSet>(this._baseUrl + service).subscribe(result => {
            this.resposta = result;
            this.pageToken = null;
            this.hasNext = this.resposta.data.nextPageToken != null;
            this.hasPrior = this.resposta.data.priorPageToken != null;
            this.loading = false;
        }, error => {
                console.error(error);
                alert('Ocorreu um erro inesperado ao pesquisar videos!');
                this.loading = false;
        });

    }

}

interface ResultSet {
    data: {
        items: {
            youtubeVideoId: string;
            youtubeChannelId: string;
            titulo: string;
            descricao: string;
            imagemUrl: string;
            tipo: number;
            publicadoEm: Date;
        }[];
        query: string;
        nextPageToken: string;
        priorPageToken: string;
    }; 
    success: boolean;
    message: string;
    pageIndex: number;
    pageSize: string;
    totalCount: number;
    totalPages: number;
    hasPreviousPage: number;
    hasNextPage: number;
}
