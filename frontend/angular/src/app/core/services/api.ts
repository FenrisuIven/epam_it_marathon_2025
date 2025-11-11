import {
  HttpClient,
  HttpParams,
  HttpResponse,
  HttpResponseBase,
} from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { Endpoint } from '../../app.enum';
import { BASE_URL } from './tokens/base-url.token';
import {
  UserDetails,
  RoomCreationRequest,
  RoomSummary,
  RoomDetails,
  User,
  JoinRoomResponse,
  LeaveRoomResponse,
  RoomUpdateRequest,
} from '../../app.models';

@Injectable({
  providedIn: 'root',
})
export class ApiService {
  readonly #baseUrl = inject(BASE_URL);
  readonly #http = inject(HttpClient);

  public createRoom(
    roomCreationData: RoomCreationRequest
  ): Observable<HttpResponse<RoomSummary>> {
    return this.#http.post<RoomSummary>(
      `${this.#baseUrl}${Endpoint.rooms}`,
      roomCreationData,
      { observe: 'response' }
    );
  }

  public getRoomByRoomCode(
    roomCode: string
  ): Observable<HttpResponse<RoomDetails>> {
    const params = new HttpParams().set('roomCode', roomCode);

    return this.#http.get<RoomDetails>(`${this.#baseUrl}${Endpoint.rooms}`, {
      params,
      observe: 'response',
    });
  }

  public addUserToRoom(
    roomCode: string,
    userData: UserDetails
  ): Observable<HttpResponse<JoinRoomResponse>> {
    const params = new HttpParams().set('roomCode', roomCode);

    const res = this.#http.post<JoinRoomResponse>(
      `${this.#baseUrl}${Endpoint.users}`,
      userData,
      { params, observe: 'response' }
    );

    console.log({ res });

    return res;
  }

  public removeUserFromRoom(
    id: number,
    userCode: string
  ): Observable<HttpResponse<string>> {
    const params = new HttpParams().set('userCode', userCode);

    return this.#http.delete<string>(
      `${this.#baseUrl}${Endpoint.users}/${id}`,
      {
        params,
        observe: 'response',
      }
    );
  }

  public getRoomByUserCode(
    userCode: string
  ): Observable<HttpResponse<RoomDetails>> {
    const params = new HttpParams().set('userCode', userCode);

    return this.#http.get<RoomDetails>(`${this.#baseUrl}${Endpoint.rooms}`, {
      params,
      observe: 'response',
    });
  }

  public getUsers(userCode: string): Observable<HttpResponse<User[]>> {
    const params = new HttpParams().set('userCode', userCode);

    return this.#http.get<User[]>(`${this.#baseUrl}${Endpoint.users}`, {
      params,
      observe: 'response',
    });
  }

  public drawNames(userCode: string): Observable<HttpResponse<string>> {
    const params = new HttpParams().set('userCode', userCode);

    return this.#http.post<string>(
      `${this.#baseUrl}${Endpoint.roomsDraw}`,
      null,
      { params, observe: 'response' }
    );
  }

  public patchRoom(
    userCode: string,
    payload: RoomUpdateRequest
  ): Observable<HttpResponse<RoomDetails>> {
    const params = new HttpParams().set('userCode', userCode);

    return this.#http.patch<RoomDetails>(
      `${this.#baseUrl}${Endpoint.rooms}`,
      payload,
      { params, observe: 'response' }
    );
  }
}
