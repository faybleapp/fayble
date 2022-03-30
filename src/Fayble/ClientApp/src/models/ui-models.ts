export interface BreadcrumbItem {
	name: string;
	link: string;
	active?: boolean;
}

export enum LibraryView {
	CoverGrid,
	List,
}

export enum MediaType {
	ComicBook,
	Book,
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

export interface SelectFieldOption {
	value: string ;
	label: string;
}
