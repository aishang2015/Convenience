import { Component, OnInit, ViewChild } from '@angular/core';
import { FormGroup, FormBuilder, FormControl, Validators } from '@angular/forms';
import { EntityControl } from '../model/entityControl';
import { NzMessageService } from 'ng-zorro-antd';
import { CodeService } from 'src/app/services/code.service';
@Component({
  selector: 'app-code-generator',
  templateUrl: './code-generator.component.html',
  styleUrls: ['./code-generator.component.scss']
})
export class CodeGeneratorComponent implements OnInit {

  editForm: FormGroup;

  controls: EntityControl[] = [];

  currentIndex = 0;

  code = '';
  editorOptions = { theme: 'vs-dark', language: 'csharp' };

  @ViewChild('editor', { static: true })
  editor;

  constructor(private formBuilder: FormBuilder,
    private messageService: NzMessageService,
    private codeService: CodeService) { }

  ngOnInit(): void {
    this.editForm = this.formBuilder.group({
      entityName: [null, [Validators.required, Validators.pattern("[a-zA-Z\$_][a-zA-Z\\d_]*$")]],
      databaseContext: [null, [Validators.required, Validators.pattern("[a-zA-Z\$_][a-zA-Z\\d_]*$")]]
    });
  }

  removeField(control) {
    const index = this.controls.indexOf(control);
    this.controls.splice(index, 1);
    this.editForm.removeControl(control.property);
    this.editForm.removeControl(control.type);
    this.editForm.removeControl(control.length);
    this.editForm.removeControl(control.isRequired);
  }

  addField() {
    const id = this.controls.length == 0 ? 1 : this.controls[this.controls.length - 1].id + 1;
    const newControl = new EntityControl(id, `property${id}`, `type${id}`, `length${id}`, `isRequired${id}`);
    this.controls.push(newControl);
    this.editForm.addControl(newControl.property, new FormControl(null, [Validators.required, Validators.pattern("[a-zA-Z\$_][a-zA-Z\\d_]*$")]));
    this.editForm.addControl(newControl.type, new FormControl(null, [Validators.required]));
    this.editForm.addControl(newControl.length, new FormControl(null));
    this.editForm.addControl(newControl.isRequired, new FormControl(null));
  }

  submitEdit() {
    for (const i in this.editForm.controls) {
      this.editForm.controls[i].markAsDirty();
      this.editForm.controls[i].updateValueAndValidity();
    }
    if (this.editForm.valid) {
      if (this.controls.length == 0) {
        this.messageService.error("至少要有一个字段!");
        return;
      }
      this.currentIndex = 1;
      let properties = [];
      this.controls.forEach(control => {
        properties.push({ type: this.editForm.value[control.type], property: this.editForm.value[control.property] })
      });
      this.code = this.codeService.getBackClass(this.editForm.value['entityName'], this.editForm.value['databaseContext'], properties);
    }
  }

  backToFirstStep() {
    this.currentIndex = 0;
  }

}
