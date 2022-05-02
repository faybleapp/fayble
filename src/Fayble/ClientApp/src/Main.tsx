import cn from "classnames";
import { LoadingIndicator } from "components/loadingIndicator";
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
import React, { useEffect, useState } from "react";
import { Route, Routes, useLocation, useNavigate } from "react-router-dom";
import { useSystemConfiguration } from "services/system";
import styles from "./Main.module.scss";

export const Main = () => {
	const [hideNavAndSidebar, setHideNavAndSidebar] = useState<boolean>(true);
	const [loading, setLoading] = useState<boolean>(true);
	const { sidebarOpen } = useAppState();

	const loggedIn = isLoggedIn();
	const navigate = useNavigate();
	const location = useLocation();
	const {
		data: systemConfiguration,
		isLoading: isLoadingSystemConfiguration,
	} = useSystemConfiguration();

	useEffect(() => {
		setLoading(isLoadingSystemConfiguration);
		console.log(systemConfiguration);
		console.log(isLoadingSystemConfiguration);
	}, [isLoadingSystemConfiguration, systemConfiguration]);

	useEffect(() => {
		const firstRun = systemConfiguration?.firstRun;
		setHideNavAndSidebar(
			loading ||
				location.pathname.toLowerCase() === "/first-run" ||
				location.pathname.toLowerCase() === "/login"
		);
		if (firstRun && location.pathname.toLowerCase() !== "/first-run") {
			navigate("/first-run");
		}
		else if (!firstRun && location.pathname.toLowerCase() === "/first-run") {
			navigate("/");
		}

	}, [location, navigate, systemConfiguration, loading]);

	// if (location.pathname.toLowerCase() === "/first-run") {
	// 	navigate(systemConfiguration?.firstRun ? "/first-run" : "/");
	// }

	// useEffect(() => {
	// 	if (
	// 		systemConfiguration?.firstRun &&
	// 		location.pathname.toLowerCase() !== "/first-run"
	// 	) {
	// 		navigate("/first-run");
	// 	}
	// }, [systemConfiguration?.firstRun, location, navigate]);

	// if (loggedIn && location.pathname.toUpperCase() === "/LOGIN") {
	// 	navigate("/");
	// }
	// if (!loggedIn && location.pathname.toUpperCase() !== "/LOGIN") {
	// 	navigate("/login");
	// }

	return (
		<div className={styles.application}>
			{loading ? (
				<LoadingIndicator />
			) : (
				<>
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
						<div
							className={styles.pageContent}
							id="main-page-content">
							<Routes>
								<Route path="/" element={<Home />} />
								<Route path="/login" element={<Login />} />
								<Route
									path="/first-run"
									element={<FirstRun />}
								/>
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
				</>
			)}
		</div>
	);
};
