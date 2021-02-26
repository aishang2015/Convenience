import { Component, OnInit, forwardRef, Renderer2, ViewChild, ElementRef } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { NzModalRef, NzModalService } from 'ng-zorro-antd/modal';

@Component({
  selector: 'app-avatar-select',
  templateUrl: './avatar-select.component.html',
  styleUrls: ['./avatar-select.component.less'],
  providers: [{
    provide: NG_VALUE_ACCESSOR,
    useExisting: forwardRef(() => AvatarSelectComponent),
    multi: true
  }]
})
export class AvatarSelectComponent implements OnInit, ControlValueAccessor {

  selectedAvatar;
  avatarList: string[] = [];

  @ViewChild('input', { static: true, read: ElementRef })
  inputElementRef: ElementRef;

  @ViewChild('avatarSelectTpl', { static: true })
  avatarSelectTpl;

  @ViewChild('avatarFooterTpl', { static: true })
  avatarFooterTpl;

  modalRef: NzModalRef;

  constructor(private _renderer: Renderer2,
    private _modalService: NzModalService) { }

  writeValue(obj: any): void {
    this.selectedAvatar = obj;
    this._renderer.setProperty(this.inputElementRef.nativeElement, 'value', obj);
  }
  registerOnChange(fn: any): void {
    this.onChange = fn;
  }
  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }
  setDisabledState?(isDisabled: boolean): void {
    this._renderer.setProperty(this.inputElementRef.nativeElement, 'disabled', isDisabled);
  }

  ngOnInit(): void {
    for (let i = 1; i <= 33; i++) {
      this.avatarList.push(i.toString());
    }
  }

  onTouched = () => { };
  onChange = _ => { };

  showAvatarModal() {
    this.modalRef = this._modalService.create({
      nzTitle: "请选择头像",
      nzContent: this.avatarSelectTpl,
      nzFooter: this.avatarFooterTpl
    });
  }

  confirm() {
    this.modalRef.close();
    this.writeValue(this.selectedAvatar);
    this.onChange(this.selectedAvatar);
  }


  getImgUrl(name) {
    return `/assets/avatars/${name}.png`;
  }

  setSelectedAvatar(name) {
    this.selectedAvatar = name;
  }
}
