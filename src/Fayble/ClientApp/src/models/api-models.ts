//----------------------
// <auto-generated>
//     Generated using the NSwag toolchain v13.15.10.0 (NJsonSchema v10.6.10.0 (Newtonsoft.Json v13.0.0.0)) (http://NSwag.org)
// </auto-generated>
//----------------------

/* tslint:disable */
/* eslint-disable */
// ReSharper disable InconsistentNaming



export interface Authentication {
    token: string;
    expiration: Date;
    loggedIn: boolean;
    loggedInUser: User;
    refreshToken: string;
}

export interface User {
    id: string;
    name: string;
    username: string;
}

export interface BackgroundTask {
    id: string;
    itemId: string | undefined;
    itemName: string;
    status: string;
    description: string | undefined;
    type: string;
}

export interface BackgroundTaskRequest {
    itemId: string;
    taskType: string;
}

export interface Book {
    id: string;
    title: string;
    summary: string;
    pageCount: number | undefined;
    mediaPath: string;
    fileName: string;
    fileType: string;
    filePath: string;
    fileSize: number;
    rating: number;
    media: Media;
    publisher: Publisher | undefined;
    read: boolean;
    created: Date;
    modified: Date;
    number: string;
    language: string;
    series: Series | undefined;
    library: Library | undefined;
    releaseDate: string | undefined;
    coverDate: string | undefined;
    lastMetadataUpdate: Date;
    mediaType: string;
    tags: string[];
}

export interface Media {
    coverFull: string | undefined;
    coverMed: string | undefined;
    coverSm: string | undefined;
}

export interface Publisher {
    id: string | undefined;
    name: string;
    description: string;
    mediaPath: string;
}

export interface Series {
    id: string;
    name: string | undefined;
    volume: string | undefined;
    summary: string | undefined;
    notes: string | undefined;
    year: number | undefined;
    bookCount: number | undefined;
    parentSeriesId: string | undefined;
    parentSeries: Series | undefined;
    publisherId: string | undefined;
    publisher: Publisher | undefined;
    rating: number;
    library: Library | undefined;
    read: boolean;
    media: Media | undefined;
    locked: boolean;
    lastMetadataUpdate: Date | undefined;
}

export interface Library {
    id: string | undefined;
    name: string;
    libraryType: string;
    paths: string[] | undefined;
    settings: LibrarySettings;
}

export interface LibrarySettings {
    reviewOnImport: boolean;
}

export interface UpdateBook {
    id: string;
    title: string | undefined;
    summary: string | undefined;
    notes: string | undefined;
    rating: number;
    number: string | undefined;
    releaseDate: string | undefined;
    coverDate: string | undefined;
    language: string | undefined;
    review: string | undefined;
    tags: string[];
}

export interface UpdateSeries {
    id: string;
    name: string | undefined;
    year: number | undefined;
    summary: string | undefined;
    notes: string | undefined;
    volume: string | undefined;
    rating: number;
    publisherId: string | undefined;
    parentSeriesId: string | undefined;
}

export interface Tag {
    id: string;
    name: string;
}

export interface FileResponse {
    data: Blob;
    status: number;
    fileName?: string;
    headers?: { [name: string]: any };
}