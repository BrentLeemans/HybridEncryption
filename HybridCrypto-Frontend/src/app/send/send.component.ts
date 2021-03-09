import { Component, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { SendService } from '../services/send.service';
import { ListUsersService } from '../services/list-users.service';

@Component({
  selector: 'app-send',
  templateUrl: './send.component.html',
  styleUrls: ['./send.component.less'],
})
export class SendComponent implements OnInit {
  sendingForm: FormGroup;
  errorMessage: string;
  isTextFile = true;
  messageGotSent = false;

  constructor(
    private fb: FormBuilder,
    private sendService: SendService,
    private listUsersService: ListUsersService
  ) {
    this.sendingForm = new FormGroup({
      senderId: new FormControl(''),
      receiverId: new FormControl('', [Validators.required]),
      text: new FormControl(''),
      file: new FormControl([]),
    });
  }

  ngOnInit(): void {
    this.listUsersService.getIsLoaded().subscribe((bool) => {
      if (bool) {
        this.sendingForm
          .get('receiverId')
          .setValue(this.listUsersService.selectedValue);
      }
    });
  }

  async readFileValue(file): Promise<any> {
    return new Promise((resolve) => {
      const fileReader = new FileReader();
      let fileInBytes = null;
      fileReader.onload = (e) => {
        // @ts-ignore
        fileInBytes = Object.values(new Int8Array(e.target.result));
        resolve(fileInBytes);
      };
      fileReader.readAsArrayBuffer(file);
    });
  }

  async send() {
    this.isTextFile = true;
    let fileData = null;
    const file = (document.getElementById('fileUpload') as HTMLInputElement)
      .files[0];
    if (file !== undefined) {
      if (file.size !== 0) {
        fileData = await this.readFileValue(file);
      } else {
        alert('file is empty!');
        return;
      }
    }

    // returns true if it matches criteria
    this.isTextFile = fileData === null ? null : !fileData.some((b) => b < 0);

    if (!this.isTextFile) {
      if (this.sendingForm.get('text').value === '') {
        return;
      }
    }

    this.sendingForm
      .get('receiverId')
      .setValue(this.listUsersService.selectedValue);
    this.sendingForm.get('file').setValue(fileData);
    this.sendService.send(this.sendingForm.value).subscribe(
      (response) => {
        this.messageGotSent = true;
      },
      () => {
        this.errorMessage = 'You can only send text files!';
      }
    );
  }

  get senderId() {
    return '';
  }

  get receiverId() {
    return this.sendingForm.get('receiverId');
  }

  get text() {
    return this.sendingForm.get('text');
  }

  get file() {
    return this.sendingForm.get('file');
  }
}
