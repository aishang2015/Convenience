import { Component, OnInit } from '@angular/core';
import { SafeResourceUrl, DomSanitizer } from '@angular/platform-browser';
import { UriConstant } from 'src/app/core/constants/uri-constant';

@Component({
  selector: 'app-swagger-ui',
  templateUrl: './swagger-ui.component.html',
  styleUrls: ['./swagger-ui.component.scss']
})
export class SwaggerUiComponent implements OnInit {

  code;

  constructor(private uriConstant: UriConstant,
    private sanitizer: DomSanitizer) { }

  ngOnInit(): void {
    let url = `${this.uriConstant.BaseUri}/swagger/index.html`;
    this.code = this.sanitizer.bypassSecurityTrustResourceUrl(url);
  }

}
