import { Component, OnInit } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { UriConfig } from 'src/app/configs/uri-config';

@Component({
  selector: 'app-hangfire-ui',
  templateUrl: './hangfire-ui.component.html',
  styleUrls: ['./hangfire-ui.component.scss']
})
export class HangfireUIComponent implements OnInit {

  code;

  constructor(private uriConstant: UriConfig,
    private sanitizer: DomSanitizer) { }

  ngOnInit(): void {
    let url = `${this.uriConstant.BaseUri}/hangfire`;
    this.code = this.sanitizer.bypassSecurityTrustResourceUrl(url);
  }

}
