import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class CodeService {

  constructor() { }

  getBackClass(entityName: string, dbcontext: string, properties: { type; property; }[]) {

    let propertyString = '';
    properties.forEach(element => {
      propertyString += `public ${this.getCsharpType(element.type)} ${this.transfer(element.property)} {get;set;}

        `;
    });
    return `namespace Convience.Entity.Entity
{
    [Entity(DbContextType = typeof(${this.transfer(dbcontext)}))]
    public class ${this.transfer(entityName)}
    {
        ${propertyString}
    }
}`;

  }


  transfer(str: string) {
    if (str.length > 0) {
      let result = str.toLowerCase();
      return result[0].toUpperCase() + result.substring(1, result.length - 1);
    }
  }

  getCsharpType(type: string) {
    let result = '';
    switch (type) {
      case 'guid':
        result = 'Guid';
        break;
      case 'int':
        result = 'int';
        break;
      case 'string':
        result = 'string';
        break;
      case 'bool':
        result = 'bool';
        break;
      case 'datetime':
        result = 'DateTime';
        break;
    }
    return result;
  }

}
