import {
  faCheckCircle,
  faCircleCheck,
  faCircleExclamation,
  faSpinner
} from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
  ColumnDef,
  createColumnHelper,
  flexRender,
  getCoreRowModel,
  useReactTable
} from "@tanstack/react-table";
import cn from "classnames";
import {
  IndeterminateCheckbox,
  IndeterminateCheckboxValue
} from "components/indeterminateCheckbox";
import { BookImport } from "models";
import { ComicFile } from "models/api-models";
import { useEffect, useState } from "react";
import { Button, OverlayTrigger, Table, Tooltip } from "react-bootstrap";
import { useAllSeries } from "services";
import { useGenerateFilename } from "services/fileSystem";
import { ImportRow } from "./components/ImportRow";
import { MatchModal } from "./components/MatchModal";
import { SelectSeriesModal } from "./components/SelectSeriesModal";
import { SetImportFilenameModal } from "./components/SetImportFilenameModal";
import { SetNumberModal } from "./components/SetNumberModal";
import styles from "./ImportTable.module.scss";

interface ImportTableProps {
  files: ComicFile[];
}

export const ImportTable = ({ files }: ImportTableProps) => {
  const [showSeriesModal, setShowSeriesModal] = useState<boolean>(false);
  const [showNumberModal, setShowNumberModal] = useState<boolean>(false);
  const [showMatchModal, setShowMatchModal] = useState<boolean>(false);
  const [queuedForImport, setQueuedForImport] = useState<Array<string>>([]);
  const [selectedFile, setSelectedFile] = useState<BookImport | undefined>();
  const [selectAllStatus, setSelectAllStatus] =
    useState<IndeterminateCheckboxValue>(IndeterminateCheckboxValue.Unchecked);
  const [showImportFilenameModal, setShowImportFilenameModal] =
    useState<boolean>(false);
  const [importFiles, setImportFiles] = useState<Array<BookImport>>(
    files.map((file, index) => {
      return {
        id: index.toString(),
        seriesId: "",
        seriesMatchId: "",
        bookId: "",
        matchId: "",
        destinationFileName: file.fileName,
        filePath: file.filePath,
        number: file.number || "",
        checked: false,
        loading: false,
        fileName: file.fileName,
        exists: false,
      };
    })
  );
  const { data: series } = useAllSeries();
  const generateFilename = useGenerateFilename();

  const updateBookImport = (updatedFile: BookImport) => {
    setImportFiles((importFiles) =>
      importFiles.map((file) => {
        if (file.id === updatedFile.id) {
          return updatedFile;
        }
        return file;
      })
    );
  };

  const checkedFiles = importFiles
    .filter((f) => f.checked)
    .map((f) => f.filePath);

  const checkedAndValid = importFiles.filter(
    (f) =>
      checkedFiles.some((cf) => cf === f.filePath) &&
      f.destinationFileName &&
      f.seriesId
  );

  useEffect(() => {
    if (checkedFiles.length === 0) {
      setSelectAllStatus(IndeterminateCheckboxValue.Unchecked);
    } else if (checkedFiles.length === importFiles.length) {
      setSelectAllStatus(IndeterminateCheckboxValue.Checked);
    } else {
      setSelectAllStatus(IndeterminateCheckboxValue.Indeterminate);
    }
  }, [checkedFiles, importFiles]);

  const columnHelper = createColumnHelper<BookImport>();

  const renderSeriesSelect = (id: string) => {
    const importFile = importFiles.find((f) => f.filePath === id);
    const seriesName = series?.find((s) => s.id === importFile?.seriesId)?.name;
    return (
      <div
        className={cn(styles.selectableCell, {
          [styles.placeholder]: !seriesName,
        })}
        onClick={() => {
          setSelectedFile(importFile);
          setShowSeriesModal(true);
        }}>
        {seriesName ?? "select"}
      </div>
    );
  };

  const renderCheckbox = (filePath: string) => {
    return !queuedForImport.some((q) => q === filePath) ? (
      <input
        type="checkbox"
        className={styles.checkbox}
        checked={checkedFiles.includes(filePath)}
        onChange={(e) => {
          setImportFiles((importFiles) =>
            importFiles.map((file) => {
              if (file.filePath === filePath) {
                return { ...file, checked: e.target.checked };
              }
              return file;
            })
          );
        }}
      />
    ) : null;
  };

  useEffect(() => {
    selectedFile &&
      setImportFiles((importFiles) =>
        importFiles.map((file) => {
          if (file.filePath === selectedFile.filePath) {
            return selectedFile;
          }
          return file;
        })
      );
  }, [selectedFile]);

  const renderHeaderCheckbox = () => {
    return (
      <IndeterminateCheckbox
        className={styles.checkbox}
        onChange={() => {
          setImportFiles((prevState) =>
            prevState.map((file) => {
              return {
                ...file,
                checked:
                  selectAllStatus === IndeterminateCheckboxValue.Unchecked,
              };
            })
          );
        }}
        value={selectAllStatus}
      />
    );
  };

  const renderImportFilename = (id: string) => {
    const importFile = importFiles.find((f) => f.filePath === id);

    return importFile?.loading ? (
      <div className={styles.generatingFilename}>Generating...</div>
    ) : (
      <div
        className={cn(styles.selectableCell, {
          [styles.placeholder]: !importFile?.destinationFileName,
        })}
        onClick={() => {
          setSelectedFile(importFile);
          setShowImportFilenameModal(true);
        }}>
        {!!importFile?.destinationFileName
          ? importFile?.destinationFileName
          : "set"}
      </div>
    );
  };

  const renderNumber = (id: string) => {
    const importFile = importFiles.find((f) => f.filePath === id);
    return importFile?.seriesId ? (
      <div
        className={cn(styles.selectableCell, {
          [styles.placeholder]: !importFile?.number,
        })}
        onClick={() => {
          setSelectedFile(importFile);
          setShowNumberModal(true);
        }}>
        {!!importFile?.number ? importFile?.number : "set"}
      </div>
    ) : (
      <></>
    );
  };

  const renderMatch = (id: string) => {
    const importFile = importFiles.find((f) => f.filePath === id);
    return importFile?.seriesMatchId ? (
      <div
        className={cn(styles.selectableCell, {
          [styles.placeholder]: !importFile?.matchId,
        })}
        onClick={() => {
          setSelectedFile(importFile);
          setShowMatchModal(true);
        }}>
        {!!importFile?.matchId ? (
          <FontAwesomeIcon icon={faCircleCheck} />
        ) : (
          "match"
        )}
      </div>
    ) : (
      <></>
    );
  };

  const renderStatus = (importFile: BookImport) => {
    if (importFile?.loading) {
      return (
        <FontAwesomeIcon className={styles.loadingIcon} spin icon={faSpinner} />
      );
    }

    let error;
    let message = "";

    if (queuedForImport.some((q) => q === importFile?.id)) {
      error = false;
      message = "Queued for import";
    } else if (importFile?.exists) {
      error = true;
      message = "A file already exists with this name";
    } else if (!importFile?.seriesId) {
      error = true;
      message = "You must select a series";
    } else if (!importFile?.number) {
      error = true;
      message = "You must set a number";
    } else if (!importFile?.destinationFileName) {
      error = true;
      message = "You must set a destination filename";
    } else {
      return null;
    }

    return (
      <OverlayTrigger placement="left" overlay={<Tooltip>{message}</Tooltip>}>
        <div>
          {error ? (
            <FontAwesomeIcon
              className={styles.errorIcon}
              icon={faCircleExclamation}
            />
          ) : (
            <FontAwesomeIcon
              className={styles.importIcon}
              icon={faCheckCircle}
            />
          )}
        </div>
      </OverlayTrigger>
    );
  };

  const handleImport = () => {
    const files = importFiles.filter(
      (f) =>
        checkedFiles.some((cf) => cf === f.filePath) &&
        f.destinationFileName &&
        f.seriesId
    );
    setQueuedForImport(files.map((f) => f.filePath));
    setImportFiles((importFiles) =>
      importFiles.map((file) => ({ ...file, checked: false }))
    );
  };

  const columns: ColumnDef<BookImport, any>[] = [
    columnHelper.accessor((row) => row, {
      id: "check",
      header: () => renderHeaderCheckbox(),
      cell: (info) => renderCheckbox(info.row.original.filePath),
    }),
    columnHelper.accessor((row) => row.fileName, {
      id: "filename",
      header: "Filename",
      cell: (info) => info.getValue(),
    }),
    columnHelper.accessor((row) => row, {
      header: "Series",
      cell: (info) => renderSeriesSelect(info.row.original.filePath),
    }),
    columnHelper.accessor((row) => row, {
      header: "Number",
      cell: (info) => renderNumber(info.row.original.filePath),
    }),
    columnHelper.accessor((row) => row, {
      id: "match",
      header: "Match",
      cell: (info) => renderMatch(info.row.original.filePath),
    }),
    columnHelper.accessor((row) => row, {
      header: "Import Filename",
      cell: (info) => renderImportFilename(info.row.original.filePath),
    }),
    columnHelper.accessor((row) => row, {
      id: "status",
      header: "",
      cell: (info) => renderStatus(info.row.original),
    }),
  ];

  const table = useReactTable({
    data: importFiles,
    columns,
    getCoreRowModel: getCoreRowModel(),
  });

  return (
    <>
      <div className={styles.importTable}>
        <Table striped hover>
          <thead>
            {table.getHeaderGroups()?.map((headerGroup) => (
              <tr key={headerGroup.id}>
                {headerGroup.headers.map((header) => (
                  <th key={header.id}>
                    {header.isPlaceholder
                      ? null
                      : flexRender(
                          header.column.columnDef.header,
                          header.getContext()
                        )}
                  </th>
                ))}
              </tr>
            ))}
          </thead>
          <tbody className={styles.tableBody}>
            {table.getRowModel().rows.map((row) => (
              <ImportRow row={row} updateBookImport={updateBookImport} />
            ))}
          </tbody>
        </Table>
      </div>
      <div className={styles.tableButtons}>
        <Button
          disabled={checkedFiles.length === 0}
          variant="secondary"
          onClick={() => {
            setSelectedFile(undefined);
            setShowSeriesModal(true);
          }}>
          Select Series
        </Button>
        <Button
          disabled={checkedAndValid.length === 0}
          className={styles.importButton}
          onClick={handleImport}>
          Import Selected
        </Button>
      </div>

      <SelectSeriesModal
        show={showSeriesModal}
        onClose={() => {
          setShowSeriesModal(false);
          setSelectedFile(undefined);
        }}
        onSelectSeries={(seriesId) => {
          const matchedSeries = series!.find((s) => s.id === seriesId);
          if (selectedFile) {
            setSelectedFile(
              (prevState) =>
                ({
                  ...prevState,
                  seriesId: matchedSeries?.id,
                  seriesMatchId: matchedSeries?.matchId,
                  matchId: undefined,
                } as BookImport)
            );
          } else {
            setImportFiles(
              importFiles.map((file) => {
                if (checkedFiles.some((f) => f === file.filePath)) {
                  return {
                    ...file,
                    seriesId: seriesId,
                    seriesMatchId: matchedSeries?.matchId,
                    matchId: undefined,
                  };
                }
                return file;
              })
            );
          }

          setShowSeriesModal(false);
        }}
      />
      <SetNumberModal
        number={selectedFile?.number || ""}
        show={showNumberModal}
        onClose={(number) => {
          setSelectedFile(
            (prevState) =>
              ({
                ...prevState,
                number: number,
              } as BookImport)
          );
          setShowNumberModal(false);
        }}
      />

      <SetImportFilenameModal
        onClose={() => {
          setShowImportFilenameModal(false);
          setSelectedFile(undefined);
        }}
        show={showImportFilenameModal}
        filename={selectedFile?.destinationFileName || ""}
        onChange={(filename) => {
          setSelectedFile(
            (prevState) =>
              ({
                ...prevState,
                destinationFileName: filename,
              } as BookImport)
          );
        }}
      />

      {selectedFile?.seriesMatchId && (
        <MatchModal
          show={showMatchModal}
          seriesMatchId={selectedFile.seriesMatchId}
          matchId={selectedFile.matchId!}
          filename={
            files?.find((f) => f.filePath === selectedFile.filePath)?.fileName
          }
          onClose={() => {
            setShowMatchModal(false);
            setSelectedFile(undefined);
          }}
          onMatch={(matchId) => {
            setSelectedFile(
              (prevState) =>
                ({
                  ...prevState,
                  matchId: matchId,
                } as BookImport)
            );
            setShowMatchModal(false);
          }}
        />
      )}
    </>
  );
};
