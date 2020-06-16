export class WorkFlowFormControl {
    id?: string;

    name?:string;
    domId?: string;
    controlType?: ControlTypeEnum;
    top?: number;
    left?: number;
    width?: number;
    fontSize?: number;

    content?: string;
    line?: number;
    optionList?: string[];
    options: string;

    isRequired?: boolean;
    parttern?: string;
    maxLength?: number;

}

export enum ControlTypeEnum {
    Label = 1,
    TextBox = 2,
    Select = 3,
    Number = 4,
    Date = 5,
    DateTime = 6,
    TextArea = 7,
}