import { Component, computed, input, output } from '@angular/core';
import { ParentModalLayout } from '../../../../core/directives/parent-modal-layout';
import { Button } from '../../button/button';
import { IconButton } from '../../icon-button/icon-button';
import { FocusTrap } from '../../../../core/directives/focus-trap';
import { fadeIn } from '../../../../utils/animations';
import { FADE_IN_ANIMATION_DURATION_MS } from '../../../../app.constants';
import { ButtonText } from '../../../../app.enum';
import { BudgetPipe } from '../../../pipes/budget.pipe';

@Component({
  selector: 'app-common-modal-template',
  imports: [Button, IconButton, FocusTrap, BudgetPipe],
  templateUrl: './common-modal-template.html',
  styleUrl: './common-modal-template.scss',
  animations: [fadeIn(FADE_IN_ANIMATION_DURATION_MS)],
})
export class CommonModalTemplate extends ParentModalLayout {
  readonly subtitle = input.required<string>();
  readonly cancelButtonText = input<ButtonText>();
  readonly budget = input<number>();

  readonly buttonType = input<string>('primary');
  readonly cancelButtonType = input<string>('secondary');

  readonly cancelButtonAction = output<void>();

  readonly isBudgetShown = computed(() => this.budget() !== undefined);

  public onCancelButtonClick(): void {
    this.cancelButtonAction.emit();
  }
}
