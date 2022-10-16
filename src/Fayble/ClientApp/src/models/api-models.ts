//----------------------
// <auto-generated>
//     Generated using the NSwag toolchain v13.15.10.0 (NJsonSchema v10.6.10.0 (Newtonsoft.Json v13.0.0.0)) (http://NSwag.org)
// </auto-generated>
//----------------------

/* tslint:disable */
/* eslint-disable */
// ReSharper disable InconsistentNaming



export interface AuthenticationResult {
    token: string;
    expiration: Date;
    isAuthenticated: boolean;
    userId: string;
    refreshToken: string;
}

export interface LoginCredentials {
    username: string;
    password: string;
}

export interface RefreshTokenRequest {
    refreshToken: string;
}

export interface BackgroundTask {
    id: string;
    itemId: string | undefined;
    itemName: string | undefined;
    status: string | undefined;
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
    fileName: string;
    fileType: string;
    filePath: string;
    fileSize: number;
    rating: number;
    mediaRoot: string;
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
    tags: string[] | undefined;
    people: BookPerson[] | undefined;
    fieldLocks: BookFieldLocks;
    isDeleted: boolean;
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
    matchId: string | undefined;
    publisher: Publisher | undefined;
    rating: number;
    library: Library | undefined;
    read: boolean;
    locked: boolean;
    mediaRoot: string;
    lastMetadataUpdate: Date | undefined;
    fieldLocks: SeriesFieldLocks;
}

export interface Library {
    id: string | undefined;
    name: string;
    libraryType: string;
    folderPath: string;
    settings: LibrarySettings;
}

export interface LibrarySettings {
    reviewOnImport: boolean;
    useComicInfo: boolean;
    yearAsVolume: boolean;
}

export interface SeriesFieldLocks {
    name: boolean;
    volume: boolean;
    summary: boolean;
    notes: boolean;
    year: boolean;
    rating: boolean;
    publisherId: boolean;
    parentSeriesId: boolean;
}

export interface Person {
    id: string | undefined;
    name: string;
}

export interface BookPerson extends Person {
    role: string;
}

export interface BookFieldLocks {
    title: boolean;
    summary: boolean;
    number: boolean;
    language: boolean;
    rating: boolean;
    releaseDate: boolean;
    coverDate: boolean;
    tags: boolean;
    authors: boolean;
    writers: boolean;
    inkers: boolean;
    editors: boolean;
    pencillers: boolean;
    letterers: boolean;
    colorists: boolean;
    coverArtists: boolean;
    translators: boolean;
    other: boolean;
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
    fieldLocks: BookFieldLocks;
    people: BookPerson[];
}

export interface RelatedBooks {
    booksInSeries: Book[] | undefined;
    booksByPublisher: Book[] | undefined;
    booksByAuthor: Book[] | undefined;
    booksByWriter: Book[] | undefined;
    booksReleasedSameMonth: Book[] | undefined;
    booksReleasedSameYear: Book[] | undefined;
}

export interface PathValidation {
    path: string;
}

export interface ComicFile {
    number: string | undefined;
    year: number | undefined;
    fileExtension: string | undefined;
    filePath: string;
    coverImage: string | undefined;
    fileName: string;
    pageCount: number;
    fileSize: number;
    fileLastModifiedDate: Date;
    comicInfoXml: ComicInfoXml | undefined;
}

export interface ComicInfoXml {
    title: string | undefined;
    series: string | undefined;
    number: string | undefined;
    volume: string | undefined;
    summary: string | undefined;
    notes: string | undefined;
    year: string | undefined;
    month: string | undefined;
    day: string | undefined;
    writer: string | undefined;
    penciller: string | undefined;
    inker: string | undefined;
    colorist: string | undefined;
    letterer: string | undefined;
    coverArtist: string | undefined;
    editor: string | undefined;
    translator: string | undefined;
    publisher: string | undefined;
    imprint: string | undefined;
    genre: string | undefined;
    tags: string | undefined;
    web: string | undefined;
    pageCount: string | undefined;
    format: string | undefined;
    characters: string | undefined;
    teams: string | undefined;
    languageIso: string | undefined;
    ageRating: string | undefined;
}

export interface ImportScan {
    path: string;
}

export interface ImportFile {
    seriesId: string | undefined;
    destinationFileName: string;
    filePath: string;
    number: string;
}

export interface SeriesSearchResult {
    id: string;
    name: string | undefined;
    summary: string | undefined;
    description: string | undefined;
    startYear: number | undefined;
    publisher: string | undefined;
    issueCount: number;
    image: string | undefined;
    levenshteinDistance: number;
}

export interface SeriesResult {
    id: string;
    name: string;
    image: string;
    description: string;
    summary: string;
    startYear: number;
    providers: ProviderResult[];
    books: BookResult[];
}

export interface ProviderResult {
    id: string;
    providerItemId: string;
    name: string;
}

export interface BookResult {
    id: string;
    title: string;
    number: string;
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
    matchId: string | undefined;
    fieldLocks: SeriesFieldLocks;
}

export interface MediaSettings {
    bookNamingConvention: string;
    comicBookStandardNamingConvention: string;
    comicBookOneShotNamingConvention: string;
    colonReplacement: string;
    renameFiles: boolean;
}

export interface FirstRun {
    ownerCredentials: LoginCredentials;
}

export interface SystemSettings {
    firstRun: boolean;
}

export interface Tag {
    id: string;
    name: string;
}

export interface User {
    id: string;
    username: string;
    role: string | undefined;
}

export interface NewUser {
    username: string;
    password: string;
    role: string;
}

export interface FileResponse {
    data: Blob;
    status: number;
    fileName?: string;
    headers?: { [name: string]: any };
}