import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.less']
})
export class HomeComponent implements OnInit {

  uri = 'https://localhost:5001/api/tenant';

  data;

  constructor(private _httpClient: HttpClient) { }

  ngOnInit(): void {
    this._httpClient.get(this.uri + '/member').subscribe(result => {
      this.data = result;
    });
  }

}
