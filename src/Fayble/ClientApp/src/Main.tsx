import cn from "classnames";
import { NavbarMenu as Navbar } from "components/navbar";
import { Sidebar } from "components/sidebar";
import { useAppState } from "context";
import { isLoggedIn } from "helpers/authenticationHelpers";
import { FirstRun } from "pages/first-run";
import { Home } from "pages/home";
import { Library } from "pages/library";
import { Book } from "pages/library/book";
import { Series } from "pages/library/series";
import { Login } from "pages/login";
import React, { useEffect } from "react";
import { Route, Routes, useLocation, useNavigate } from "react-router-dom";
import styles from "./Main.module.scss";

export const Main = () => {
	const { sidebarOpen } = useAppState();

	const loggedIn = isLoggedIn();
	const navigate = useNavigate();
	const location = useLocation();
	// TODO: UseConfiguration

	const firstRun = true;

	useEffect(() => {
		if (firstRun && location.pathname.toLowerCase() !== "/first-run") {
			navigate("/first-run");
		}
	}, [location, firstRun, navigate]);

	const hideNavAndSidebar = firstRun;

	// if (loggedIn && location.pathname.toUpperCase() === "/LOGIN") {
	// 	navigate("/");
	// }
	// if (!loggedIn && location.pathname.toUpperCase() !== "/LOGIN") {
	// 	navigate("/login");
	// }

	return (
		<div className={styles.application}>
			{hideNavAndSidebar ? null : (
				<div
					className={cn(
						styles.sidebar,
						sidebarOpen ? styles.open : styles.closed
					)}>
					<Sidebar />
				</div>
			)}
			<div className={styles.main}>
				{hideNavAndSidebar ? null : <Navbar />}
				<div className={styles.pageContent} id="main-page-content">
					<Routes>
						<Route path="/" element={<Home />} />
						<Route path="/login" element={<Login />} />
						<Route path="/first-run" element={<FirstRun />} />
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
