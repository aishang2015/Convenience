import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class CodeService {

  constructor() { }

  getBackClass(entityName: string, dbcontext: string, properties: { type; property; }[]) {

    let propertyString = '';
    properties.forEach(element => {
      propertyString += `public ${element.type} ${element.property} {get;set;}

      `;
    });
    return `namespace Convience.Entity.Entity
  {
      [Entity(DbContextType = typeof(${dbcontext}))]
      public class ${entityName}
      {
          ${propertyString}
      }
  }`;

  }
}
