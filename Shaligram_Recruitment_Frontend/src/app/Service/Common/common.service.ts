import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { StorageKey, StorageService } from '../Storage/storage.service';
import { catchError, Observable, throwError } from 'rxjs';
import { ApiResponse } from '../../Models/CommonModel';
import { environment } from '../../Environments/environment';
import * as CryptoJS from 'crypto-js';

@Injectable({
  providedIn: 'root'
})
export class CommonService {
  //Constructor
  constructor(private http: HttpClient, private storageService: StorageService) { }

  private key = CryptoJS.MD5(CryptoJS.enc.Utf8.parse('EA34FF3E-6DF2-4551-B59E-BB81D9564426'));
  private iv = { ...this.key };

  //For HTTPGET Methods
  // doGet(apiUrl: string): Observable<ApiResponse> {
  //   const httpOptions = {
  //     headers: new HttpHeaders()
  //   };
  //   let loginData = this.storageService.getValue(StorageKey.authKey);

  //   if (loginData) {
  //     httpOptions.headers = httpOptions.headers.set('Authorization', `Bearer ${loginData}`);

  //   }

  //   const url = `${environment.BaseURL}${apiUrl}`;
  //   console.log("CollegeBatch"+url);

  //   return this.http.get<ApiResponse>(url).pipe(
  //     catchError((error) => {
  //       return throwError(() => error); // Handle error response
  //     })
  //   );
  // }
  doGet(apiUrl: String): Observable<ApiResponse> {
    const httpOptions = {
      headers: new HttpHeaders()
    };
    let loginData = this.storageService.getValue(StorageKey.authKey)
    // loginData = loginData != null ? this.Decrypt(loginData) : null;
    if (loginData) {
      httpOptions.headers = httpOptions.headers.set('Authorization', 'Bearer ' + loginData);
      httpOptions.headers = httpOptions.headers.set('Access-Control-Allow-Origin', '*');
      httpOptions.headers = httpOptions.headers.set('Access-Control-Allow-Methods', 'POST, GET, OPTIONS, DELETE, PUT');
    }
    const url = `${environment.BaseURL}${apiUrl}`;
    return this.http.get<ApiResponse>(url, httpOptions);
  }
  //For HTTPDELETE methods
  //For HTTPDELETE methods
  // doDelete(apiUrl: String, idtoDelete?: number): Observable<ApiResponse> {
  //   const httpOptions = {
  //     headers: new HttpHeaders(
  //     )
  //   };
  //   let loginData = this.storageService.getValue(StorageKey.authKey)

  //   if (loginData) {
  //     httpOptions.headers = httpOptions.headers.set('Authorization', 'Bearer ' + loginData);
  //     httpOptions.headers = httpOptions.headers.set('Access-Control-Allow-Origin', '*');
  //     httpOptions.headers = httpOptions.headers.set('Access-Control-Allow-Methods', 'POST, GET, OPTIONS, DELETE, PUT');
  //   }
  //   const url = (`${environment.BaseURL}${apiUrl}`);
  //   console.log("Delete data "+url);

  //   return this.http.delete<ApiResponse>(url);
  // }
  doDelete(apiUrl: String, idtoDelete?: number): Observable<ApiResponse> {
    const httpOptions = {
      headers: new HttpHeaders(
      )
    };
    let loginData = this.storageService.getValue(StorageKey.authKey)

    if (loginData) {
      httpOptions.headers = httpOptions.headers.set('Authorization', 'Bearer ' + loginData);
      httpOptions.headers = httpOptions.headers.set('Access-Control-Allow-Origin', '*');
      httpOptions.headers = httpOptions.headers.set('Access-Control-Allow-Methods', 'POST, GET, OPTIONS, DELETE, PUT');
    }
    const url = (`${environment.BaseURL}${apiUrl}`);
    return this.http.delete<ApiResponse>(url, httpOptions);
  }

  //For HTTPPOST methods
  // doPost(apiUrl: string, postData: any): Observable<ApiResponse> {
  //   const httpOptions = {
  //     headers: new HttpHeaders(
  //     )
  //   };
  //   let loginData = this.storageService.getValue(StorageKey.authKey)
  //   if (loginData) {
  //     httpOptions.headers = httpOptions.headers.set('Authorization', 'Bearer ' + loginData);
  //   }
  //   const url = `${environment.BaseURL}${apiUrl}`;


  //   return this.http.post<ApiResponse>(url, postData);
  // }
  //For HTTPPOST methods
  doPost(apiUrl: string, postData: any): Observable<ApiResponse> {
    const httpOptions = {
      headers: new HttpHeaders(
      )
    };
    let loginData = this.storageService.getValue(StorageKey.authKey)

    if (loginData) {
      httpOptions.headers = httpOptions.headers.set('Authorization', 'Bearer ' + loginData);
    }
    const url = `${environment.BaseURL}${apiUrl}`;
    console.log(url);
    
    return this.http.post<ApiResponse>(url, postData, httpOptions);
  }

  //For Encryption perpose
  public Encrypt(clearText: string): string {
    this.iv.sigBytes = 8;
    var encrypted = CryptoJS.TripleDES.encrypt(CryptoJS.enc.Utf8.parse(clearText), this.key,
      {
        keySize: 128 / 8,
        iv: this.iv,
        mode: CryptoJS.mode.ECB,
        padding: CryptoJS.pad.Pkcs7
      });
    return encrypted.toString();
  }

  //For Decryption perpose
  public Decrypt(cipherText: string): string {
    if (cipherText) {
      this.iv.sigBytes = 8;
      var decrypted = CryptoJS.TripleDES.decrypt(cipherText, this.key,
        {
          keySize: 128 / 8,
          iv: this.iv,
          mode: CryptoJS.mode.ECB,
          padding: CryptoJS.pad.Pkcs7
        });
      return decrypted.toString(CryptoJS.enc.Utf8);

    } else {
      return '""';
    }
  }

  IsLoggedIn() {
    return (this.storageService.getValue('Token') != null && this.storageService.getValue('EncryptedEmail') != null) ? this.storageService.getValue('Token') : '';
  }
  IsNonAuthLoggedIn() {
    return this.storageService.getValue('EncryptedEmail') != null ? this.storageService.getValue('EncryptedEmail') : '';
  }
}
