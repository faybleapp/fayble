export interface BreadcrumbItem {
	name: string;
	link: string;
	active?: boolean;
}

export enum ViewType {
	CoverGrid,
	List,
}

export enum MediaType {
	ComicBook = "ComicBook",
	Book = "Book",
}

export enum BackgroundTaskType {
	LibraryScan = "LibraryScan",
	SeriesScan = "SeriesScan",
}

export enum BackgroundTaskStatus {
	Queued = "Queued",
    Running = "Running",
    Complete = "Complete",
    Failed = "Failed"
}

export enum ImageTypes {
	CoverSm = "cover-250.jpg",
	CoverMed = "cover-500.jpg",
	CoverFull = "cover.jpg"
}

export interface SelectFieldOption {
	value: string ;
	label: string;
}

export interface HorizontalListItem {
	id: string;
	title: string;
	subtitle?: string;
	image: string;
	link: string;
}