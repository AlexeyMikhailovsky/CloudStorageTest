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

  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    http.get<MyFile[]>(baseUrl + 'file/getlist').subscribe(result => {
      this.myfiles = result;
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
