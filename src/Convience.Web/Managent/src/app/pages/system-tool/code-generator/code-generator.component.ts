import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, FormControl, Validators } from '@angular/forms';
import { EntityControl } from '../model/entityControl';
import { CodeService } from 'src/app/business/code.service';
import { saveAs } from 'file-saver';
import * as JSZip from 'jszip';
import { NzMessageService } from 'ng-zorro-antd/message';

@Component({
  selector: 'app-code-generator',
  templateUrl: './code-generator.component.html',
  styleUrls: ['./code-generator.component.less']
})
export class CodeGeneratorComponent implements OnInit {

  editForm: FormGroup;

  controls: EntityControl[] = [];

  currentIndex = 0;

  radioValue = 0;

  radioList: { name: string; value: number; }[] = [];

  isGenerateFinish: boolean = false;

  // code: {
  //   bentity?: string; 
  //   bconfig?: string; 
  //   bms?: string; 
  //   bvmv?: string; 
  //   bqv?: string;
  //   bservice?: string; 
  //   bcontroller?: string; 
  //   fmodel?: string; 
  //   fservice?: string; 
  //   fhtml?: string; 
  //   fts?: string;
  // } = {};
  // codeKeys: string[] = ['bentity', 'bconfig', 'bms', 'bvmv', 'bqv', 'bservice', 'bcontroller',
  //   'fmodel', 'fservice', 'fhtml', 'fts'];
  code: string[] = [];

  fileNameList: string[] = [];

  editorOptions = { theme: 'vs-dark', language: 'csharp' };

  editCode = '';

  properties = [];

  constructor(
    private _formBuilder: FormBuilder,
    private _messageService: NzMessageService,
    private _codeService: CodeService) { }

  ngOnInit(): void {
    this.editForm = this._formBuilder.group({
      entityName: [null, [Validators.required, Validators.pattern("[a-zA-Z\$_][a-zA-Z\\d_]*$")]]
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
        this._messageService.error("至少要有一个字段!");
        return;
      }
      this.makeCodes();
    }
  }

  makeCodes() {

    this.isGenerateFinish = false;
    this.fileNameList = this._codeService.getFileNameList(this.editForm.value['entityName']);
    this.radioList = [];
    this.fileNameList.forEach(element => {
      this.radioList.push({ name: element, value: this.radioList.length });
    });

    this.currentIndex = 1;
    this.controls.forEach(control => {
      this.properties.push({
        type: this.editForm.value[control.type],
        property: this.editForm.value[control.property],
        isRequired: this.editForm.value[control.isRequired],
        length: this.editForm.value[control.length]
      })
    });

    this.code[0] ??= this._codeService.getBackEntity(this.editForm.value['entityName'], this.properties);

    this.isGenerateFinish = true;
  }

  toThirdStep() {
    this.generateFile();
    this.currentIndex = 2;
  }

  // 选中文件发生变化
  radioChange(index) {
    switch (index) {
      case 0:
        this.code[0] ??= this._codeService.getBackEntity(this.editForm.value['entityName'], this.properties);
        break;
      case 1:
        this.code[1] ??= this._codeService.getBackEntityConfig(this.editForm.value['entityName'], this.properties);
        break;
      case 2:
        this.code[2] ??= this._codeService.getBackModels(this.editForm.value['entityName'], this.properties);
        break;
      case 3:
        this.code[3] ??= this._codeService.getBackViewModelValidator(this.editForm.value['entityName'], this.properties);
        break;
      case 4:
        this.code[4] ??= this._codeService.getBackQueryValidator(this.editForm.value['entityName']);
        break;
      case 5:
        this.code[5] ??= this._codeService.getBackService(this.editForm.value['entityName'], this.properties);
        break;
      case 6:
        this.code[6] ??= this._codeService.getBackController(this.editForm.value['entityName']);
        break;
      case 7:
        this.code[7] ??= this._codeService.getFrontModel(this.editForm.value['entityName'], this.properties);
        break;
      case 8:
        this.code[8] ??= this._codeService.getFrontService(this.editForm.value['entityName'], this.properties);
        break;
      case 9:
        this.code[9] ??= this._codeService.getFrontHtml(this.editForm.value['entityName'], this.properties);
        break;
      case 10:
        this.code[10] ??= this._codeService.getFrontTs(this.editForm.value['entityName'], this.properties);
        break;
    }
  }


  generateFile() {
    let zip = new JSZip();
    this.code[0] ??= this._codeService.getBackEntity(this.editForm.value['entityName'], this.properties);
    this.code[1] ??= this._codeService.getBackEntityConfig(this.editForm.value['entityName'], this.properties);
    this.code[2] ??= this._codeService.getBackModels(this.editForm.value['entityName'], this.properties);
    this.code[3] ??= this._codeService.getBackViewModelValidator(this.editForm.value['entityName'], this.properties);
    this.code[4] ??= this._codeService.getBackQueryValidator(this.editForm.value['entityName']);
    this.code[5] ??= this._codeService.getBackService(this.editForm.value['entityName'], this.properties);
    this.code[6] ??= this._codeService.getBackController(this.editForm.value['entityName']);
    this.code[7] ??= this._codeService.getFrontModel(this.editForm.value['entityName'], this.properties);
    this.code[8] ??= this._codeService.getFrontService(this.editForm.value['entityName'], this.properties);
    this.code[9] ??= this._codeService.getFrontHtml(this.editForm.value['entityName'], this.properties);
    this.code[10] ??= this._codeService.getFrontTs(this.editForm.value['entityName'], this.properties);

    for (let index in this.fileNameList) {
      let content = this.code[index];
      zip.file(this.fileNameList[index], content);
    }

    zip.generateAsync({ type: "blob" }).then(function (content) {
      saveAs(content, "code.zip");
    });
  }

  reset() {
    this.radioValue = 0;
    this.currentIndex = 0;
    this.controls = [];
    this.editForm = this._formBuilder.group({
      entityName: [null, [Validators.required, Validators.pattern("[a-zA-Z\$_][a-zA-Z\\d_]*$")]],
      databaseContext: [null, [Validators.required, Validators.pattern("[a-zA-Z\$_][a-zA-Z\\d_]*$")]]
    });
  }

  backToFirstStep() {
    this.radioValue = 0;
    this.currentIndex = 0;
  }

}
