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
    public resposta: ResultSet;
    public searchQuery: string;
    public pageSize: number;
    public pageIndex: number;
    public displayPageIndex: number;
    public loading: boolean;

    //pageSize: this.pageSize, pageIndex: this.pageIndex, searchQuery: this.searchQuery

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
        if (this.pageIndex > 0) {
            service = service + '&pageIndex=' + this.pageIndex;
        }
        if (this.searchQuery != null) {
            service = service + '&queryString=' + this.searchQuery;
        }

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

    gotoDetails(canal: Canal) {
        let canalId = canal ? canal.id : null;

        if (this.searchQuery != null) {
            this.router.navigate(['/canal-details', { id: canalId, pageSize: this.pageSize, pageIndex: this.pageIndex, searchQuery: this.searchQuery }]);
        }
        else {
            this.router.navigate(['/canal-details', { id: canalId, pageSize: this.pageSize, pageIndex: this.pageIndex }]);
        }
    }

    public onPageSizeChange(newValue) {
        this.pageSize = newValue;
        this.pageIndex = 0;
        this.startSearch();
    }

}

interface Canal {
    id: string;
    channelId: string;
    title: string;
    descricao: string;
    imagem: string;
    publicadoEm: Date;
}

interface ResultSet {
    data: Canal[];
    success: boolean;
    message: string;
    pageIndex: number;
    pageSize: string;
    totalCount: number;
    totalPages: number;
    hasPreviousPage: number;
    hasNextPage: number;
}
