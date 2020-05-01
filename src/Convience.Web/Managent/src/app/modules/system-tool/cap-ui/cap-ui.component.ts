import { Component, OnInit } from '@angular/core';
import { UriConstant } from 'src/app/core/constants/uri-constant';
import { DomSanitizer } from '@angular/platform-browser';

@Component({
  selector: 'app-cap-ui',
  templateUrl: './cap-ui.component.html',
  styleUrls: ['./cap-ui.component.scss']
})
export class CapUiComponent implements OnInit {

  code;

  constructor(private uriConstant: UriConstant,
    private sanitizer: DomSanitizer) { }

  ngOnInit(): void {
    let url = `${this.uriConstant.BaseUri}/cap`;
    this.code = this.sanitizer.bypassSecurityTrustResourceUrl(url);
  }

}
