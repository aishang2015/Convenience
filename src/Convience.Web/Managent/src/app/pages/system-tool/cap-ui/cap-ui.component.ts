import { Component, OnInit } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { UriConfig } from 'src/app/configs/uri-config';

@Component({
  selector: 'app-cap-ui',
  templateUrl: './cap-ui.component.html',
  styleUrls: ['./cap-ui.component.scss']
})
export class CapUiComponent implements OnInit {

  code;

  constructor(private uriConstant: UriConfig,
    private sanitizer: DomSanitizer) { }

  ngOnInit(): void {
    let url = `${this.uriConstant.BaseUri}/cap`;
    this.code = this.sanitizer.bypassSecurityTrustResourceUrl(url);
  }

}
