import { MainLayout } from "components/layout/main";
import { LoadingIndicator } from "components/loadingIndicator";
import { FirstRun } from "pages/first-run";
import { Import } from "pages/import";
import { Library } from "pages/library";
import { Book } from "pages/library/book";
import { Series } from "pages/library/series";
import { Login } from "pages/login";
import { useRoutes } from "react-router-dom";
import { useSystemConfiguration } from "services/system";

export const Routes = () => {
  const { data: systemConfiguration, isLoading } = useSystemConfiguration();
  const routes = useRoutes([
    {
      path: "/",
      element: <MainLayout />,
      children: [
        {
          path: "/library/:libraryId",
          element: <Library />,
        },
        {
          path: "/library/:libraryId/series/:seriesId",
          element: <Series />,
        },
        {
          path: "/library/:libraryId/book/:bookId",
          element: <Book />,
        },
        { path: "/import", element: <Import /> },
      ],
    },
    { path: "/login", element: <Login /> },
  ]);

  const generateRoutes = () => {
    if (systemConfiguration?.firstRun) {
      return <FirstRun />;
    }

    return routes;
  };

  return <>{isLoading ? <LoadingIndicator /> : generateRoutes()}</>;
};
