import { Component, Inject, EventEmitter, OnInit, Output } from '@angular/core';
import { HttpClient, HttpEventType, HttpErrorResponse, HttpResponse } from '@angular/common/http';

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html'
})
export class FetchDataComponent implements OnInit {
  public progress!: number;
  public message!: string;
  @Output() public onUploadFinished = new EventEmitter();
  public myfiles: MyFile[] = [];
  public myfolders: MyFolder[] = [];

  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    http.get<MyFile[]>(baseUrl + 'file/getlist').subscribe(result => {
      this.myfiles = result;
    }, error => console.error(error));
    http.get<MyFolder[]>(baseUrl + 'file/getfolder').subscribe(result => {
      this.myfolders = result;
    }, error => console.error(error));
  }

  ngOnInit() {
  }

  uploadFile = (files: any) => {
    if (files.length === 0) {
      return;
    }
    let fileToUpload = <File>files[0];
    const formData = new FormData();
    formData.append('file', fileToUpload, fileToUpload.name);

    this.http.post('http://localhost:44476/file/upload', formData, { reportProgress: true, observe: 'events' })
      .subscribe({
        next: (event) => {
          if (event.type === HttpEventType.UploadProgress)
            this.progress = Math.round(100 * event.loaded / 100);
          else if (event.type === HttpEventType.Response) {
            this.message = 'Upload success.';
            this.onUploadFinished.emit(event.body);
          }
        },
        error: (err: HttpErrorResponse) => console.log(err)
      });
  }

  downloadFile(route: number, filename: any): void{
    const baseUrl = 'http://localhost:44476/file/download';
    this.http.get(baseUrl + route, { responseType: 'blob' as 'json' }).subscribe(
      (response: any) => {
        let dataType = response.type;
        let binaryData = [];
        binaryData.push(response);
        let downloadLink = document.createElement('a');
        downloadLink.href = window.URL.createObjectURL(new Blob(binaryData, { type: dataType }));
        if (filename)
          downloadLink.setAttribute('download', filename);
        document.body.appendChild(downloadLink);
        downloadLink.click();
      }
    )
  }

  deleteFile(route: number) {
    const baseUrl = 'http://localhost:44476/file/deleteFile';
    this.http.delete(baseUrl + route).subscribe(
      (response: any) => {
        console.log(response);
      }
    )
  }

  deleteFolder(route: number) {
    const baseUrl = 'http://localhost:44476/file/deletefolder';
    this.http.delete(baseUrl + route).subscribe(
      (response: any) => {
        console.log(response);
      }
    )
  }

  getFolder(route: number) {
    const baseUrl = 'http://localhost:44476/file/getfolder';
    this.http.get<MyFolder[]>(baseUrl + route).subscribe(result => {
      this.myfolders = result;
    }, error => console.error(error));

    this.http.get<MyFile[]>(baseUrl + '/getFilesFromFolder/' + route).subscribe(result => {
      this.myfiles = result;
    }, error => console.error(error));
  }
}

interface MyFile {
  id: number;
  idFolder: number;
  location: string;
  name: string;
  creationDate: string;
  uploadDate: string;
  type: string;
  size: string;
}

interface MyFolder {
  id: number;
  idParent: number;
  name: string;
}
