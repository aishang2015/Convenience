import { Directive, Input, TemplateRef, ViewContainerRef } from '@angular/core';

@Directive({ selector: '[canOperate]' })
export class CanOperateDirective {

    constructor(
        private templateRef: TemplateRef<any>,
        private viewContainer: ViewContainerRef) { }

    @Input() set canOperate(identification: string) {

        // 判断是否包含此元素的操作权
        let hasRight = true;

        if (hasRight) {
            this.viewContainer.createEmbeddedView(this.templateRef);
        } else{
            this.viewContainer.clear();
        }
    }
}