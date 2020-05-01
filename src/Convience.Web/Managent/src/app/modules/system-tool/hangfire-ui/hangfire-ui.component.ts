import { Component, OnInit } from '@angular/core';
import { UriConstant } from 'src/app/core/constants/uri-constant';
import { DomSanitizer } from '@angular/platform-browser';

@Component({
  selector: 'app-hangfire-ui',
  templateUrl: './hangfire-ui.component.html',
  styleUrls: ['./hangfire-ui.component.scss']
})
export class HangfireUIComponent implements OnInit {

  code;

  constructor(private uriConstant: UriConstant,
    private sanitizer: DomSanitizer) { }

  ngOnInit(): void {
    let url = `${this.uriConstant.BaseUri}/hangfire`;
    console.log(url);
    this.code = this.sanitizer.bypassSecurityTrustResourceUrl(url);
    console.log(this.code);
  }

}
