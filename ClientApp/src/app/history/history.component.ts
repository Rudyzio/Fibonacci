import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-history',
  templateUrl: './history.component.html'
})
export class HistoryComponent {
  public entries: HistoricEntry[];
  public paginationMetadata: PaginationMetadata;
  public links: Link[];

  public previousPage: string;
  public nextPage: string;

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
    this.callBackend(this.baseUrl + 'fibonacci');
  }

  onChangePage(url: string) {
    this.callBackend(url);
  }

  callBackend(url: string) {
    this.http.get<any>(url, { observe: 'response' })
      .subscribe(response => {

        this.paginationMetadata = JSON.parse(response.headers.get('X-Pagination'));

        this.entries = response.body.result;
        this.links = response.body.links;

        this.previousPage = null;
        this.nextPage = null;

        if (this.links.find(x => x.rel === 'previousPage')) {
          this.previousPage = this.links.find(x => x.rel === 'previousPage').href;
        }

        if (this.links.find(x => x.rel === 'nextPage')) {
          this.nextPage = this.links.find(x => x.rel === 'nextPage').href;
        }


      }, error => console.error(error));
  }
}

interface HistoricEntry {
  input: number;
  result: string;
  dateCreated: Date;
}

interface Link {
  rel: string;
  href: string;
  method: string
}

interface PaginationMetadata {
  totalCount: number;
  pageSize: number;
  currentPage: number;
  totalPages: number;
}
