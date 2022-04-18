import cn from "classnames";
import { NavbarMenu as Navbar } from "components/navbar";
import { Sidebar } from "components/sidebar";
import { useAppState } from "context/AppStateContext";
import { Home } from "pages/home";
import { Library } from "pages/library";
import { Book } from "pages/library/book";
import { Series } from "pages/library/series";
import React from "react";
import { Route, Routes } from "react-router-dom";
import styles from "./Main.module.scss";

export const Main = () => {
	const { sidebarOpen } = useAppState();
	return (
		<div className={styles.application}>
			<Sidebar />
			<div
				className={cn(
					styles.main,
					sidebarOpen ? styles.sidebaropen : styles.sidebarclosed
				)}>
				<Navbar />
				<div className={styles.pageContent} id="main-page-content">
					<Routes>
						<Route path="/" element={<Home />} />
						<Route
							path="/library/:libraryId"
							element={<Library />}
						/>
						<Route
							path="/library/:libraryId/series/:seriesId"
							element={<Series />}
						/>
						<Route
							path="/library/:libraryId/series/:seriesId/book/:bookId"
							element={<Book />}
						/>
					</Routes>
				</div>
			</div>
		</div>
	);
};
