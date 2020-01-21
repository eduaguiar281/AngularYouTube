import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
    selector: 'app-canal',
    templateUrl: './canal.component.html'
})
export class CanalComponent {
    constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string, private route: ActivatedRoute,
        private router: Router) {
        this._baseUrl = baseUrl;
        this._http = http;
        this.loading = false;
        this.pageSize = 5;
        this.pageIndex = 0;
    }
    _http: HttpClient;
    _baseUrl: string;
    public resposta: ResultSet;
    public searchQuery: string;
    public pageSize: number;
    public pageIndex: number;
    public displayPageIndex: number;
    public loading: boolean;

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
        var service = 'api/v1/Canal/?pageSize=' + this.pageSize;// + this.searchQuery;
        if (this.pageIndex > 0)
            service = service + '&pageIndex=' + this.pageIndex;
        if (this.searchQuery != null)
            service = service + '&queryString=' + this.searchQuery;

        this._http.get<ResultSet>(this._baseUrl + service).subscribe(result => {
            this.resposta = result;
            this.displayPageIndex = result.pageIndex + 1;
            this.pageIndex = result.pageIndex;
            this.loading = false;
        }, error => {
            console.error(error);
            alert('Ocorreu um erro inesperado ao pesquisar canais!');
            this.loading = false;
        });

    }

}

interface ResultSet {
    data: {
        id: string;
        channelId: string;
        title: string;
        descricao: string;
        imagem: string;
        publicadoEm: Date;
    }[];
    success: boolean;
    message: string;
    pageIndex: number;
    pageSize: string;
    totalCount: number;
    totalPages: number;
    hasPreviousPage: number;
    hasNextPage: number;
}
