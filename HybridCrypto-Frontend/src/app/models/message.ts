export class Message {
  senderId: string;
  receiverId: string;
  text: string;
  file: Uint8Array;
  date: Date;
}
