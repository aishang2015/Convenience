import { Component, OnInit, ViewChild } from '@angular/core';
import { FormGroup, FormBuilder, FormControl, Validators } from '@angular/forms';
import { EntityControl } from '../model/entityControl';
import { NzMessageService } from 'ng-zorro-antd';
import { CodeService } from 'src/app/services/code.service';
import { saveAs } from 'file-saver';

@Component({
  selector: 'app-code-generator',
  templateUrl: './code-generator.component.html',
  styleUrls: ['./code-generator.component.scss']
})
export class CodeGeneratorComponent implements OnInit {

  editForm: FormGroup;

  controls: EntityControl[] = [];

  currentIndex = 0;

  radioValue = 0;

  radioList: { name: string; value: number; }[] = [];

  code: {
    bentity?: string; bconfig?: string; bvm?: string; br?: string; bq?: string; bvmv?: string; bqv?: string;
    bservice?: string; biservice?: string; bcontroller?: string; fmodel?: string; fservice?: string; fhtml?: string; fts?: string;
  } = {};
  codeKeys: string[] = ['bentity', 'bconfig', 'bvm', 'br', 'bq', 'bvmv', 'bqv', 'bservice', 'biservice', 'bcontroller',
    'fmodel', 'fservice', 'fhtml', 'fts'];

  fileNameList: string[] = [];

  editorOptions = { theme: 'vs-dark', language: 'csharp' };

  editCode = '';

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
      this.makeCodes();
    }
  }

  makeCodes() {

    this.fileNameList = this.codeService.getFileNameList(this.editForm.value['entityName']);
    this.radioList = [];
    this.fileNameList.forEach(element => {
      this.radioList.push({ name: element, value: this.radioList.length });
    });

    this.currentIndex = 1;
    let properties = [];
    this.controls.forEach(control => {
      properties.push({
        type: this.editForm.value[control.type],
        property: this.editForm.value[control.property],
        isRequired: this.editForm.value[control.isRequired],
        length: this.editForm.value[control.length]
      })
    });

    this.code.bentity = this.codeService.getBackEntity(this.editForm.value['entityName'], this.editForm.value['databaseContext'], properties);
    this.code.bconfig = this.codeService.getBackEntityConfig(this.editForm.value['entityName'], properties);
    this.code.bvm = this.codeService.getBackViewModel(this.editForm.value['entityName'], properties);
    this.code.br = this.codeService.getBackResult(this.editForm.value['entityName']);
    this.code.bq = this.codeService.getBackQuery(this.editForm.value['entityName']);
    this.code.bvmv = this.codeService.getBackViewModelValidator(this.editForm.value['entityName']);
    this.code.bqv = this.codeService.getBackQueryValidator(this.editForm.value['entityName']);
    this.code.bservice = this.codeService.getBackService(this.editForm.value['entityName'], this.editForm.value['databaseContext']);
    this.code.biservice = this.codeService.getBackIService(this.editForm.value['entityName']);
    this.code.bcontroller = this.codeService.getBackController(this.editForm.value['entityName']);

    this.code.fmodel = this.codeService.getFrontModel(this.editForm.value['entityName'], properties);
    this.code.fservice = this.codeService.getFrontService(this.editForm.value['entityName']);
    this.code.fhtml = this.codeService.getFrontHtml(this.editForm.value['entityName']);
    this.code.fts = this.codeService.getFrontTs(this.editForm.value['entityName']);
  }

  toThirdStep() {
    this.generateFile();
    this.currentIndex = 2;
  }


  generateFile() {
    var JSZip = require("jszip");
    let zip = new JSZip();

    for (let index in this.fileNameList) {
      let content = this.code[this.codeKeys[index]];
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
    this.editForm = this.formBuilder.group({
      entityName: [null, [Validators.required, Validators.pattern("[a-zA-Z\$_][a-zA-Z\\d_]*$")]],
      databaseContext: [null, [Validators.required, Validators.pattern("[a-zA-Z\$_][a-zA-Z\\d_]*$")]]
    });
  }

  backToFirstStep() {
    this.radioValue = 0;
    this.currentIndex = 0;
  }

}
