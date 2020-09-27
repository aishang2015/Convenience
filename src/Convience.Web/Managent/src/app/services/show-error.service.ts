import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import { NzMessageService } from 'ng-zorro-antd/message';
import { debounceTime } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class ShowErrorService {

  private errorMessageSource = new Subject<string>();

  private missionAnnounced$ = this.errorMessageSource.asObservable()

  constructor(private _messageService: NzMessageService) {
    this.missionAnnounced$.pipe(debounceTime(800)).subscribe(msg => this._messageService.error(msg));
  }

  publishError(msg) {
    this.errorMessageSource.next(msg);
  }
}
