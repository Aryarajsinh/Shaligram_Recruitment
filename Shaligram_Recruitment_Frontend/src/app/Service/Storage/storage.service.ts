import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class StorageService {

  constructor() { }

  // For Get Value From Local Storage
  getValue(key: string): any {
    return localStorage.getItem(key);
  }

  //Set Value in local Storage
  setValue(key: string, value: string): void {
    localStorage.setItem(key, value);
  }

  //Remove Item from local storage
  removeValue(key: string): void {
    localStorage.removeItem(key);
  }

  clearStorage(){
    localStorage.clear();
  }
}

export class StorageKey{
  public static authKey = 'Token'
}
