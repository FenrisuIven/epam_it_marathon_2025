import { Component, input, output } from '@angular/core';

import { CommonModalTemplate } from '../../../shared/components/modal/common-modal-template/common-modal-template';
import { ButtonText, ModalTitle, PictureName } from '../../../app.enum';

@Component({
  selector: 'app-remove-participant-modal',
  imports: [CommonModalTemplate],
  templateUrl: './remove-participant-modal.html',
  styleUrl: './remove-participant-modal.scss',
})
export class RemoveParticipantModal {
  readonly participantFullName = input.required();

  readonly closeModal = output<void>();
  readonly buttonAction = output<void>();

  public readonly pictureName = PictureName.Car;
  public readonly title = ModalTitle.RemoveParticipant;
  public readonly buttonText = ButtonText.Remove;
  public readonly cancelButtonText = ButtonText.Cancel;
  public readonly subtitle = '';

  public readonly buttonType = 'warning';
  public readonly cancelButtonType = 'secondary';

  public onCloseModal(): void {
    this.closeModal.emit();
  }

  public onActionButtonClick(): void {
    this.buttonAction.emit();
  }
}
