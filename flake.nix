{
  description = "Advent of Code 2023";
  inputs = {
    flake-utils.url = "github:numtide/flake-utils";
    nixpkgs.url = "github:NixOS/nixpkgs/nixos-unstable";
    nix-ld = {
      url = "github:Mic92/nix-ld";
      inputs.nixpkgs.follows = "nixpkgs";
    };
  };

  outputs = {
    self,
    nixpkgs,
    flake-utils,
    nix-ld,
  }:
    flake-utils.lib.eachDefaultSystem (
      system: let
        pkgs = nixpkgs.legacyPackages.${system};
      in
        # Conditionally include Swift and Apple SDK for Darwin systems
        let
          darwinDeps =
            if system == "x86_64-darwin" || system == "aarch64-darwin"
            then [
              pkgs.swift
              pkgs.darwin.apple_sdk.frameworks.Foundation
              pkgs.darwin.apple_sdk.frameworks.CryptoKit
              pkgs.darwin.apple_sdk.frameworks.GSS
            ]
            else [];
        in let
          deps = darwinDeps ++ [pkgs.zlib pkgs.zlib.dev pkgs.openssl pkgs.icu];
        in {
          devShells = {
            default = pkgs.mkShell {
              NIX_LD_LIBRARY_PATH = pkgs.lib.makeLibraryPath ([
                  pkgs.stdenv.cc.cc
                ]
                ++ deps);
              NIX_LD = "${pkgs.stdenv.cc.libc_bin}/bin/ld.so";
              buildInputs = with pkgs;
                [
                  (with dotnetCorePackages;
                    combinePackages [
                      dotnet-sdk_8
                      dotnetPackages.Nuget
                    ])
                ]
                ++ [pkgs.alejandra];
            };
          };
        }
    );
}
