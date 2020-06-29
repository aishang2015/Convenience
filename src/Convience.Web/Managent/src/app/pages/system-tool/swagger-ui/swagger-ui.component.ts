import { Component, OnInit } from '@angular/core';
import { SafeResourceUrl, DomSanitizer } from '@angular/platform-browser';
import { UriConfig } from 'src/app/configs/uri-config';

@Component({
  selector: 'app-swagger-ui',
  templateUrl: './swagger-ui.component.html',
  styleUrls: ['./swagger-ui.component.scss']
})
export class SwaggerUiComponent implements OnInit {

  code;

  constructor(private uriConstant: UriConfig,
    private sanitizer: DomSanitizer) { }

  ngOnInit(): void {
    let url = `${this.uriConstant.BaseUri}/swagger/index.html`;
    this.code = this.sanitizer.bypassSecurityTrustResourceUrl(url);
  }

}
