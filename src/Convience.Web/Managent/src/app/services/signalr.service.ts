import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { StorageService } from './storage.service';
import { HubConnectionState } from '@microsoft/signalr';
import { UriConfig } from '../configs/uri-config';

@Injectable({
  providedIn: 'root'
})
export class SignalrService {

  private static connection: signalR.HubConnection;

  constructor(private _storageService: StorageService,
    private _uriConfig: UriConfig) {
    if (!SignalrService.connection) {
      var loginToken = this._storageService.userToken;
      SignalrService.connection = new signalR.HubConnectionBuilder()
        .withUrl(`${this._uriConfig._baseUri}/hubs`, { accessTokenFactory: () => loginToken })
        .configureLogging(signalR.LogLevel.Information)
        .build();
      SignalrService.connection.onclose(async () => {
        await this.start();
      });
    }
  }

  // 连接
  async start() {
    if (SignalrService.connection.state == HubConnectionState.Disconnected) {
      try {
        await SignalrService.connection.start();
        console.log("connected");
      } catch (err) {
        console.log(err);
        setTimeout(() => this.start(), 5000);
      }
    }
  }

  // 绑定响应服务器消息的处理方法
  addReceiveMessageHandler(eventName: string, newMethod: (...args: any[]) => void) {
    SignalrService.connection?.off(eventName);
    SignalrService.connection?.on(eventName, newMethod);
  }

  // 删除响应服务器消息的处理方法
  removeReceiveMessageHandler(eventName: string,) {
    SignalrService.connection?.off(eventName);
  }

  // 调用服务端hub内的方法,有返回值的promise
  invokeHubMethod(methodName: string, ...args: any[]) {
    return SignalrService.connection?.invoke(methodName, args);
  }

  // 调用服务端hub内的方法，无返回值的promise
  sendHubMethod(methodName: string, ...args: any[]) {
    return SignalrService.connection?.send(methodName, args);
  }


}
