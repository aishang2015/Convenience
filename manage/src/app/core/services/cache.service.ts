import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class CacheService {

  constructor() {
    if (localStorage.getItem('cacheMaxCount') == null) {
      localStorage.setItem('cacheMaxCount', '100');
    }
  }

  readonly length: number;

  public setObject(key: string, value: any): void {
    key = 'cache:' + key;
    this.setKeyList(key);
    localStorage[key] = JSON.stringify(value);
  }

  public getObject(key: string): any {
    key = 'cache:' + key;
    return JSON.parse(localStorage[key] || '{}');
  }

  public setMaxCount(count: number) {
    localStorage.setItem('cacheMaxCount', count.toString());
  }

  private setKeyList(key: string) {

    var keys = localStorage.getItem('cacheKeys');
    var keyArray = keys.split(',');
    keyArray.push(key);

    var maxcount = Number(localStorage.getItem('cacheMaxCount'));
    while (keyArray.length > maxcount) {
      keyArray.pop();
    }

    var newKeys = keyArray.join(',');
    localStorage.setItem('cacheKeys', newKeys);
  }




}
