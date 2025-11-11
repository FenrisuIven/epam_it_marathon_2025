import { ComponentRef, Type } from '@angular/core';
import { Observable, Subscription } from 'rxjs';
import { FormArray, FormControl, FormGroup } from '@angular/forms';
import { UrlTree } from '@angular/router';

import {
  BaseLabel,
  CaptionMessage,
  ErrorMessage,
  RadioButtonLabel,
  StepLabel,
  TextareaLabel,
  MessageType,
} from './app.enum';
import { Message } from './shared/components/message/message';
import { GifteeInfoModal } from './room/components/giftee-info-modal/giftee-info-modal';
import { MyWishlistModal } from './room/components/my-wishlist/components/my-wishlist-modal/my-wishlist-modal';
import { PersonalInfoModal } from './room/components/personal-info-modal/personal-info-modal';
import { InvitationModal } from './shared/components/invitation-modal/invitation-modal';
import { ParticipantInfoModal } from './room/components/participant-info-modal/participant-info-modal';

export interface StepperItem {
  isActive: boolean;
  isFilled: boolean;
  label: StepLabel;
}

export type InputLabel = BaseLabel | RadioButtonLabel | TextareaLabel;

export type FieldHintMessage = CaptionMessage | ErrorMessage;

export interface MessageOptions {
  message: string;
  type: MessageType;
}

export interface PopupInstance {
  ref?: ComponentRef<Message> | null;
  subscription?: Subscription;
  timerId?: ReturnType<typeof setTimeout>;
}

export type StyleMap = Record<string, string>;

export type BudgetControl = number | null;

export interface CreateRoomFormType {
  name: FormControl<string>;
  description: FormControl<string>;
  giftExchangeDate: FormControl<string>;
  giftMaximumBudget: FormControl<BudgetControl>;
}

export interface AddYourDetailsFormType {
  firstName: FormControl<string>;
  lastName: FormControl<string>;
  phone: FormControl<string>;
  email: FormControl<string>;
  deliveryInfo: FormControl<string>;
}

export interface GiftIdeaFormItem {
  name: FormControl<string>;
  infoLink: FormControl<string>;
}

export interface GiftIdeaFormType {
  wishList: FormArray<FormGroup<GiftIdeaFormItem>>;
}

export interface SurpriseGiftFormType {
  interests: FormControl<string>;
}

export interface RoomCreationRequest {
  room: BasicRoomDetails;
  adminUser: UserDetails;
}

export interface BasicRoomDetails {
  name: string;
  description: string;
  giftExchangeDate: string;
  giftMaximumBudget: number;
}

export interface UserDetails extends BasicUserDetails {
  wantSurprise: boolean;
  interests: string;
  wishList: WishListItem[];
}

export interface WishListItem {
  name: string;
  infoLink: string;
}

export interface BasicUserDetails {
  firstName: string;
  lastName: string;
  phone: string;
  email: string;
  deliveryInfo: string;
}

export interface RoomBase extends BasicRoomDetails {
  id: number;
  createdOn: string;
  modifiedOn: string;
  adminId: number;
  invitationCode: string;
  invitationNote: string;
}

export interface RoomDetails extends RoomBase {
  closedOn?: string;
  isFull: boolean;
}

export interface RoomSummary {
  room: RoomBase;
  userCode: string;
}

export interface CreateRoomSuccessPageData {
  userCode: string;
  invitationCode: string;
  invitationNote: string;
  name: string;
}

export interface JoinRoomWelcomePageData {
  giftMaximumBudget: number;
  invitationCode: string;
  giftExchangeDate: string;
  name: string;
}

export interface NavigationLinks {
  absoluteUrl: string;
  routerPath: string;
}

export type GuardReturnType = Observable<boolean | UrlTree> | boolean | UrlTree;

export type CustomError = Record<string, boolean>;

export interface User extends Partial<UserDetails> {
  id: number;
  createdOn: string;
  modifiedOn: string;
  roomId: number;
  isAdmin: boolean;
  firstName: string;
  lastName: string;
  userCode?: string;
  giftToUserId?: number;
}

export type JoinRoomResponse = Required<Omit<User, 'giftToUserId'>>;

export interface LeaveRoomResponse {
  room: RoomSummary;
}

export interface ModalEntry {
  component: ModalComponentType;
  inputs?: ModalInputs;
  outputs?: ModalOutputs;
}

export type ModalEntryNullable = ModalEntry | null;

export type ModalInputs =
  | GifteeInfoModalInputs
  | MyWishlistModalInputs
  | PersonalInfoModalInputs
  | InvitationModalInputs;

export type ModalOutputs = Record<string, (...args: unknown[]) => void>;

export type ModalComponentType = Type<
  | GifteeInfoModal
  | MyWishlistModal
  | PersonalInfoModal
  | InvitationModal
  | ParticipantInfoModal
>;

export interface GifteePersonalInfoItem {
  term: string;
  value: string;
}

export interface GifteeWishlistInfo {
  interests: string;
  wishList: WishListItem[];
}

export interface GifteeInfoModalInputs {
  personalInfo: GifteePersonalInfoItem[];
  wishListInfo: GifteeWishlistInfo;
}

export interface MyWishlistModalInputs {
  wishListInfo: GifteeWishlistInfo;
  budget?: number;
}

export interface PersonalInfoModalInputs {
  personalInfo: GifteePersonalInfoItem[];
}
export interface InvitationModalInputs {
  roomLink: string;
  invitationNote: string;
  userCode: string;
}

export interface LottieConfig {
  container: HTMLElement;
  path: string;
  loop?: boolean;
  autoplay?: boolean;
  speed?: number;
  onComplete?: () => void;
}

export interface RoomUpdateRequest extends Partial<BasicRoomDetails> {
  invitationNote?: string;
}
