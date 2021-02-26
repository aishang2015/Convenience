import { Component, OnInit } from '@angular/core';
import { UriConfig } from 'src/app/configs/uri-config';
import { DomSanitizer } from '@angular/platform-browser';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-manage-ui',
  templateUrl: './manage-ui.component.html',
  styleUrls: ['./manage-ui.component.less']
})
export class ManageUiComponent implements OnInit {

  code;

  constructor(
    private _uriConstant: UriConfig,
    private _sanitizer: DomSanitizer,
    private _activatedRoute: ActivatedRoute) { }

  ngOnInit(): void {

    this._activatedRoute.data.subscribe(data => {
      let uri = data.uri;
      let url = `${this._uriConstant._baseUri}/${uri}`;
      this.code = this._sanitizer.bypassSecurityTrustResourceUrl(url);
    });

  }

}
