import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private url ='http://10.13.106.18/api/Admin/menu';

  constructor(private client:HttpClient) { }

  fetchProducts(){
    return this.client.get<any[]>(this.url);
  }

  // handleDelete(product: any){
  //   return this.client.delete<any[]>(this.url);
  // }
}
