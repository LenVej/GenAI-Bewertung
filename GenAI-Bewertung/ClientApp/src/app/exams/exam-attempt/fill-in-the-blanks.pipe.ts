import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'fillInTheBlanks'
})
export class FillInTheBlanksPipe implements PipeTransform {
  transform(text: string): any[] {
    const parts = text.split(/({{\d+}})/g);
    return parts.map(part => {
      const match = part.match(/{{(\d+)}}/);
      if (match) {
        return { isGap: true, index: +match[1] };
      }
      return { text: part };
    });
  }
}
