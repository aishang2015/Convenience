import { Directive, Input, TemplateRef, ViewContainerRef } from '@angular/core';
import { StorageService } from '../services/storage.service';

@Directive({ selector: '[canOperate]' })
export class CanOperateDirective {

    constructor(
        private templateRef: TemplateRef<any>,
        private viewContainer: ViewContainerRef,
        private storageService: StorageService) { }

    @Input() set canOperate(identification: string) {

        let roles = this.storageService.UserRoles.split(',');
        let identifications = this.storageService.Identifycation.split(',');

        // 判断是否包含此元素的操作权
        let hasRight = roles.find(r => r == '1') ? true : identifications.find(i => i == identification);

        if (hasRight) {
            this.viewContainer.createEmbeddedView(this.templateRef);
        } else {
            this.viewContainer.clear();
        }
    }
}