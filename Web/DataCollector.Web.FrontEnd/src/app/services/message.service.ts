import { Injectable } from '@angular/core';

/**
 * The service which holds the app messages.
 */
@Injectable()
export class MessageService {
  messages: string[] = [];

  /**
   * Adds the message to the messages collection.
   * @param message is the text which will be shown in the browser
   */
  log(message: string) {
    this.messages.push(message);
    console.log(message);
  }

  /**
   * Clears log messages.
   */
  clear() {
    this.messages = [];
  }
}
