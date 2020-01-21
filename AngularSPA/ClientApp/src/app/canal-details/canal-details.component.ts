import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute, Router, ParamMap } from '@angular/router';
import { Location } from '@angular/common';

@Component({
    selector: 'app-canal-details',
    templateUrl: './canal-details.component.html'
})
export class CanalDetailsComponent {
    constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string, private route: ActivatedRoute,
        private router: Router, private _location: Location) {
        this._baseUrl = baseUrl;
        this._http = http;
        this.loading = false;

        this.id = this.route.snapshot.paramMap.get('id');
        
        var service = 'api/v1/Canal/' + this.id;// + this.searchQuery;
        this._http.get<ResultSet>(this._baseUrl + service).subscribe(result => {
            this.resposta = result;
            this.loading = false;
        }, error => {
            console.error(error);
            alert('Ocorreu um erro inesperado ao pesquisar canais!');
            this.loading = false;
        });
    }

    backClicked() {
        this._location.back();
    }

    _http: HttpClient;
    _baseUrl: string;
    public resposta: ResultSet;
    public loading: boolean;
    public id: string;
}

interface ResultSet {
    data: {
        id: string
        channelId: string;
        title: string;
        descricao: string;
        imagem: string;
        publicadoEm: Date;
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
